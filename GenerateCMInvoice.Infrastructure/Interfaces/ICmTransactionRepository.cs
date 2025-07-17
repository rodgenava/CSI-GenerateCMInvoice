using GenerateCMInvoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Infrastructure.Interfaces
{
    public interface ICmTransactionRepository
    {
        Task<List<ValidTransactions>> Getvalidtransactions(Serilog.ILogger cycleLogger, string CMstartDate);
    }
}
