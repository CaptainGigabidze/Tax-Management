using Microsoft.EntityFrameworkCore;
using TaxManagement.DAL.Models;

namespace TaxManagement.DAL.Context
{
    public class TaxManagementContext : DbContext
    {
        public TaxManagementContext(DbContextOptions<TaxManagementContext> options) : base(options) { }

        public DbSet<TaxRate> TaxRates { get; set; }
    }
}
