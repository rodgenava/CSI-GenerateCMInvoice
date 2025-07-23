using GenerateCMInvoice.Application.Configuration;
using GenerateCMInvoice.Application.Interfaces;
using GenerateCMInvoice.Application.Services;
using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace CSI_GenerateCMInvoice
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<Worker> _logger;
        private readonly FileSettings _fileSettings;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(55);

        public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger, IOptions<FileSettings> fileSettings)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _fileSettings = fileSettings.Value;
        }

        private Serilog.ILogger CreateCycleLogger()
        {
            string logFilePath = Path.Combine(_fileSettings.InvoiceLogs, $"cminvoice_job_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath)
                .CreateLogger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Scheduled Job Service started.");

                // Create a new cycle log for this execution
                var cycleLogger = CreateCycleLogger();

                const string ExecutingMessage = "Executing stored procedure...";
                _logger.LogWarning(ExecutingMessage);
                cycleLogger.Warning(ExecutingMessage);

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        
                        var cmTransactionService = scope.ServiceProvider.GetRequiredService<ICmTransactionService>();
                        var storedProcedureService = scope.ServiceProvider.GetRequiredService<IStoredProcedureService>();

                        // Step 1: Get valid transactions that are eligible for invoice generation
                        List<ValidTransactions> validTrans = await cmTransactionService.Getvalidtransactions(cycleLogger);

                        // Step 2: Execute stored procedure for each valid transaction and generate corresponding file
                        await storedProcedureService.ExecuteAndGenerateFile(cycleLogger, validTrans);
                    }
                    //test
                    const string procedureMessage = "Stored procedure successfully executed.";
                    _logger.LogInformation(procedureMessage);
                    cycleLogger.Information(procedureMessage);
                }
                catch (Exception ex)
                {
                    // Log any exceptions during the stored procedure execution
                    _logger.LogError(ex, "Error executing stored procedure.");
                    cycleLogger.Error(ex, "Error executing stored procedure.");
                }

                // Log the current run and wait for the next interval
                _logger.LogInformation($"Waiting for {Interval.TotalMinutes} Minutes(s)...");

                // Delay the next execution based on configured interval
                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}
