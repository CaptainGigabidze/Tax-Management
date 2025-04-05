using TaxManagement.DAL.Models;

namespace TaxManagement.API.Outputs
{
    public class TaxRateOutput
    {
        public string Municipality { get; set; } = string.Empty;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Rate { get; set; }
        public string Type { get; set; }
    }
}
