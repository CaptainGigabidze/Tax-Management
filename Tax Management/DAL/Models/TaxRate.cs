using Microsoft.EntityFrameworkCore;

namespace TaxManagement.DAL.Models
{
    [Index(nameof(Municipality), nameof(StartDate), nameof(EndDate), nameof(Type), IsUnique = true)]
    public class TaxRate
    {
        public int Id { get; set; }
        public string Municipality { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Rate { get; set; }
        public TaxRateType Type { get; set; }
    }

    public enum TaxRateType
    {
        Daily, Weekly, Monthly, Yearly
    }
    
}