namespace TaxManagement.API.Inputs
{
    public class TaxRateInput
    {
        public string Municipality { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Rate { get; set; }
        public string Type { get; set; }
    }
}
