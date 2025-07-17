using GenerateCMInvoice.Domain.Models;

namespace GenerateCMInvoice.Application.Interfaces
{
    public interface IFileService
    {
        Task GenerateTextFileAsync(List<CMInvoice> results, Serilog.ILogger cycleLogger);
    }
}
