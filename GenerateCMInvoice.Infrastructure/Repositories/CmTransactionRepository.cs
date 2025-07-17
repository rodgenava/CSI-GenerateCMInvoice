using GenerateCMInvoice.Domain.Models;
using GenerateCMInvoice.Infrastructure.Data;
using GenerateCMInvoice.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Infrastructure.Repositories
{
    public class CmTransactionRepository : ICmTransactionRepository
    {
        private readonly AppDBContext _context;
        private readonly ILogger<StoredProcedureRepository> _logger;

        public CmTransactionRepository(AppDBContext context, ILogger<StoredProcedureRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<ValidTransactions>> Getvalidtransactions(Serilog.ILogger cycleLogger, string CMstartDate)
        {
            _logger.LogInformation("Getvalidtransactions started.");

            var parts = CMstartDate.Split(',').Select(p => int.Parse(p.Trim())).ToArray();
            DateTime startDate = new DateTime(parts[0], parts[1], parts[2]);// Start from July 1, 2025
            DateTime today = DateTime.Today;

            try
            {
                // Step 1: Get raw valid transactions from the database
                _logger.LogInformation("Querying raw transactions from TBLcmTransactions...");

                var rawTransactions = await _context.TBLcmTransactions
                    .Where(cmt =>
                        !string.IsNullOrEmpty(cmt.CustomerCode) &&
                        !string.IsNullOrEmpty(cmt.JobOrderNo) &&
                        !cmt.IsDeleted &&
                        cmt.Status == 2)    // status in tbl_cm_transaction is SUBMITTED
                    .GroupBy(cmt => new { cmt.Location, cmt.TransactionDate })
                    .Select(g => new
                    {
                        g.Key.Location,
                        g.Key.TransactionDate
                    })
                    .ToListAsync(); // Execute in SQL
                
                // Then filter in memory to replicate the SQL logic exactly
                var validTransactions = rawTransactions
                    .Select(vt => new
                    {
                        vt.Location,
                        vt.TransactionDate,
                        TransactionDateConverted = TryConvertToDate((int)vt.TransactionDate),
                        Generated = _context.GeneratedCMInvoice.Any(gi =>
                            gi.LocationId == vt.Location &&
                            gi.TransactionDate == TryConvertToDate((int)vt.TransactionDate)) ? 1 : 0
                    })
                    .Where(vt => vt.TransactionDateConverted >= startDate
                              && vt.TransactionDateConverted <= DateTime.Today
                              && vt.Generated == 0)
                    .Select(a=> new ValidTransactions
                    {
                        Location = a.Location,
                        TransactionDate = (int)a.TransactionDate
                    })
                    .ToList();
//SQL script converted
//WITH ValidTransactions AS(
//SELECT
//    cmt.Location,
//    cmt.TransactionDate,
//    CASE
//        WHEN EXISTS(
//            SELECT 1
//            FROM tbl_cm_generated_invoice gi
//            WHERE gi.LocationId = cmt.Location
//              AND gi.TransactionDate = CAST(cmt.TransactionDate AS VARCHAR(6))
//        ) THEN 1
//        ELSE 0
//    END AS Generated
//FROM tbl_cm_transaction cmt
//WHERE cmt.JobOrderNo != ''
//  AND cmt.IsDeleted = 0
//  AND cmt.Status = 2  //	-- status in tbl_cm_transaction is SUBMITTED
//  AND TRY_CONVERT(DATE, '20' + RIGHT('000000' + CAST(cmt.TransactionDate AS VARCHAR(6)), 6), 112)
//      BETWEEN '2025-06-01' AND CAST(GETDATE() AS DATE)
//GROUP BY cmt.Location, cmt.TransactionDate
//                )

//-- Final SELECT to show only those with a matching record in tbl_cm_generated_invoice
//SELECT vt.*
//FROM ValidTransactions vt
//where vt.Generated = 0
                _logger.LogInformation("{0} valid transactions.", validTransactions.Count());

                return validTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Getvalidtransactions.");
                return new List<ValidTransactions>();
            }
        }
        private DateTime TryConvertToDate(int yymmdd)
        {
            string firstTwoDigitofYear = DateTime.Now.Year.ToString().Substring(0,2);
            var dateStr = firstTwoDigitofYear + yymmdd.ToString("D6");
            return DateTime.ParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
