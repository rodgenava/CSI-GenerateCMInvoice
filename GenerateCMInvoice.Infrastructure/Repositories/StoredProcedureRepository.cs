using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Data;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GenerateCMInvoice.Infrastructure.Repositories
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly AppDBContext _context;
        private readonly ILogger<StoredProcedureRepository> _logger;

        public StoredProcedureRepository(AppDBContext context, ILogger<StoredProcedureRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CMInvoice>> ExecuteStoredProcedureAsync(string storeCode, string transacDate, Serilog.ILogger cycleLogger)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(storeCode) || string.IsNullOrWhiteSpace(transacDate))
            {
                _logger.LogWarning("Invalid parameters: storeCode or transacDate is null or empty.");
                cycleLogger.Information("Invalid parameters: storeCode or transacDate is null or empty.");
                return new List<CMInvoice>();// Return empty list for invalid input
            }

            try
            {
                // Execute the stored procedure using FromSqlRaw with safe parameter substitution
                var result = await _context.StoredProcedureResults
                                    .FromSqlRaw("EXEC dbo.usp_CMAutoGenerateInvoice @storeCode = {0}, @TransacDate = {1}", storeCode, transacDate)
                                    .ToListAsync();

                // Log how many records were returned
                _logger.LogInformation("Stored procedure executed successfully. Returned {Count} records.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                // Log the error and return an empty result set
                _logger.LogError(ex, "Error executing stored procedure usp_CMAutoGenerateInvoice");
                return new List<CMInvoice>();
            }
        }
    }
}
