using GenerateCMInvoice.Domain.Models;

namespace GenerateCMInvoice.Application.Interfaces
{
    public interface IStoredProcedureService
    {
        Task ExecuteAndGenerateFile(Serilog.ILogger cycleLogger, List<ValidTransactions> ValidTrans);
    }
}
