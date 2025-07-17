using GenerateCMInvoice.Application.Configuration;
using GenerateCMInvoice.Application.Interfaces;
using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Application.Services
{
    public class CmTransactionService : ICmTransactionService
    {
        private readonly ICmTransactionRepository _repository;
        private readonly DateSettings _dateSettings;

        public CmTransactionService(ICmTransactionRepository repository, IOptions<DateSettings> dateSettings)
        {
            _repository = repository;
            _dateSettings = dateSettings.Value;
        }
        public async Task<List<ValidTransactions>> Getvalidtransactions(Serilog.ILogger cycleLogger)
        {
            var results = await _repository.Getvalidtransactions(cycleLogger, _dateSettings.CMstartDate);
            return results;
        }
    }
}
