using GenerateCMInvoice.Application.Configuration;
using GenerateCMInvoice.Application.Interfaces;
using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Application.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly FileSettings _fileSettings;
        public FileService(ILogger<FileService> logger, IOptions<FileSettings> fileSettings)
        {
            _logger = logger;
            _fileSettings = fileSettings.Value;
        }
        public async Task GenerateTextFileAsync(List<CMInvoice> results, Serilog.ILogger cycleLogger)
        {
            var filename = results.Select(x => x.FILENAME).FirstOrDefault();
            var content = string.Join(Environment.NewLine, results.Select(result =>
                $"{result.HDR_TRX_NUMBER}|{result.HDR_TRX_DATE}|{result.HDR_PAYMENT_TYPE}|{result.HDR_BRANCH_CODE}|{result.HDR_CUSTOMER_NUMBER}|{result.HDR_CUSTOMER_SITE}|{result.HDR_PAYMENT_TERM}|{result.HDR_BUSINESS_LINE}|{result.HDR_BATCH_SOURCE_NAME}|{result.HDR_GL_DATE}|{result.HDR_SOURCE_REFERENCE}|{result.DTL_LINE_DESC}|{result.DTL_QUANTITY}|{result.DTL_AMOUNT}|{result.DTL_VAT_CODE}|{result.DTL_CURRENCY}|{result.INVOICE_APPLIED}|{result.FILENAME}|"
            ));

            string filePath = Path.Combine(_fileSettings.InvoicePath, filename);
            await File.AppendAllTextAsync(filePath, content + Environment.NewLine);

            string filePathBkp = Path.Combine(_fileSettings.InvoicePathBkp, filename);
            await File.AppendAllTextAsync(filePathBkp, content + Environment.NewLine);

            _logger.LogInformation("Text file generated successfully at {FilePath}", filePath);
            cycleLogger.Information("Text file generated successfully at {FilePath}", filePath);
        }
    }
}
