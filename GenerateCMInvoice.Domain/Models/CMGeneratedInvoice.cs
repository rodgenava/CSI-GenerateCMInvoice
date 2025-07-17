using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Domain.Models
{
    public class CmGeneratedInvoice
    {
        public long? Id { get; set; }
        public string CustomerCode { get; set; }
        public int LocationId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string JobOrderNo { get; set; }
        public string OrigInvoiceNo { get; set; }
        public string CmInvoiceNo { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string ReferenceNo { get; set; }
        public string FileName { get; set; }
        public string GeneratedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
