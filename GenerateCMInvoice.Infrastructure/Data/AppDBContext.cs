using GenerateCMInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GenerateCMInvoice.Infrastructure.Data
{
    public  class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            StoredProcedureResults = Set<CMInvoice>();
            TBLcmTransactions = Set<CMTransactions>();
            GeneratedCMInvoice = Set<CmGeneratedInvoice>();
        }
        public DbSet<CMInvoice> StoredProcedureResults { get; set; }
        public DbSet<CMTransactions> TBLcmTransactions { get; set; }
        public DbSet<CmGeneratedInvoice> GeneratedCMInvoice { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CMInvoice>()
            .HasNoKey();
            modelBuilder.Entity<CMTransactions>().ToTable("tbl_cm_transaction");
            modelBuilder.Entity<CmGeneratedInvoice>().ToTable("tbl_cm_generated_invoice");
            
        }
    }
}
