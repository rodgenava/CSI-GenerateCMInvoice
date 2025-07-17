using GenerateCMInvoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Application.Interfaces
{
    public interface ICmTransactionService
    {
        Task<List<ValidTransactions>> Getvalidtransactions(Serilog.ILogger cycleLogger);
    }
}
