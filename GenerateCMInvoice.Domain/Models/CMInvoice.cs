using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCMInvoice.Domain.Models
{
    public class CMInvoice
    {
        public string? HDR_TRX_NUMBER { get; set; } = string.Empty;
        public string? HDR_TRX_DATE { get; set; } = string.Empty;
        public string? HDR_PAYMENT_TYPE { get; set; } = string.Empty;
        public string? HDR_BRANCH_CODE { get; set; } = string.Empty;
        public string? HDR_CUSTOMER_NUMBER { get; set; } = string.Empty;
        public string? HDR_CUSTOMER_SITE { get; set; } = string.Empty;
        public string? HDR_PAYMENT_TERM { get; set; } = string.Empty;
        public string? HDR_BUSINESS_LINE { get; set; } = string.Empty;
        public string? HDR_BATCH_SOURCE_NAME { get; set; } = string.Empty;
        public string? HDR_GL_DATE { get; set; } = string.Empty;
        public string? HDR_SOURCE_REFERENCE { get; set; } = string.Empty;
        public string? DTL_LINE_DESC { get; set; } = string.Empty;
        public int? DTL_QUANTITY { get; set; }
        public decimal? DTL_AMOUNT { get; set; }
        public string? DTL_VAT_CODE { get; set; } = string.Empty;
        public string? DTL_CURRENCY { get; set; } = string.Empty;
        public string? INVOICE_APPLIED { get; set; } = string.Empty;
        public string? FILENAME { get; set; } = string.Empty;
        public int? Id { get; set; }
        public string? CustomerId { get; set; } = string.Empty;
        public int? LocationId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? OrderNo { get; set; } = string.Empty;
        public string? GeneratedBy { get; set; } = string.Empty;
        public DateTime? LastUpdatedDate { get; set; }
        public string? LastUpdatedBy { get; set; } = string.Empty;
        
    }
}
