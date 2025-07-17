using GenerateCMInvoice.Application.Interfaces;
using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Data;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using System.ComponentModel.Design;

namespace GenerateCMInvoice.Application.Services
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly IStoredProcedureRepository _repository;
        private readonly AppDBContext _context;
        private readonly IFileService _fileService;
        private readonly ILogger<StoredProcedureService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);
        public StoredProcedureService(IStoredProcedureRepository repository, IFileService fileService, ILogger<StoredProcedureService> logger, AppDBContext context)
        {
            _repository = repository;
            _fileService = fileService;
            _logger = logger;
            _context = context;
        }

        public async Task ExecuteAndGenerateFile(Serilog.ILogger cycleLogger, List<ValidTransactions> validTrans)
        {
            // Iterate through each valid transaction entry
            foreach (var transaction in validTrans)
            {
                try
                {
                    // Log the start of stored procedure execution for the current transaction
                    _logger.LogInformation("Executing stored procedure for Location: {Location}, TransactionDate: {TransactionDate}",
                        transaction.Location, transaction.TransactionDate);

                    // Call the stored procedure using repository
                    var results = await _repository.ExecuteStoredProcedureAsync(
                        transaction.Location.ToString(),
                        transaction.TransactionDate.ToString(),
                        cycleLogger
                    );

                    // Log the result count from stored procedure execution
                    _logger.LogInformation("Stored procedure executed for Location: {Location}, TransactionDate: {TransactionDate}. Retrieved {Count} records.",
                        transaction.Location, transaction.TransactionDate, results.Count);

                    cycleLogger.Information("Stored procedure executed for Location: {Location}, TransactionDate: {TransactionDate}. Retrieved {Count} records.",
                        transaction.Location, transaction.TransactionDate, results.Count);

                    // Generate text file only if there are records
                    if (results.Count > 0)
                    {
                        // Pass the results to file generator service
                        await _fileService.GenerateTextFileAsync(results, cycleLogger);
                    }

                    _logger.LogInformation($"Interval for {Interval.TotalSeconds} Second(s)...");
                    await Task.Delay(Interval);
                }
                catch (Exception ex)
                {
                    // Log error to both system logger and cycle log in case of failure
                    _logger.LogError(ex, "Error occurred while processing Location: {Location}, TransactionDate: {TransactionDate}.",
                        transaction.Location, transaction.TransactionDate);

                    cycleLogger.Error(ex, "Error occurred while processing Location: {Location}, TransactionDate: {TransactionDate}.",
                        transaction.Location, transaction.TransactionDate);
                }
            }
        }

    }
}
