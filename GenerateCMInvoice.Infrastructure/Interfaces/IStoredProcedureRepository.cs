using GenerateCMInvoice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Infrastructure.Interfaces
{
    public interface IStoredProcedureRepository
    {
        Task<List<CMInvoice>> ExecuteStoredProcedureAsync(string storeCode, string TransacDate, Serilog.ILogger cycleLogger);
    }
}
