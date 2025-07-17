﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Domain.Models
{
    public class CMTransactions
    {
        public long Id { get; set; }
        public long Seq { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public int Location { get; set; }
        public decimal TransactionDate { get; set; }
        public string MembershipNo { get; set; } = string.Empty;
        public string RegisterNo { get; set; } = string.Empty;
        public string CashierNo { get; set; } = string.Empty;
        public string TrxNo { get; set; } = string.Empty;
        public string? JobOrderNo { get; set; } = string.Empty;
        public DateTime? OrigTranDate { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
