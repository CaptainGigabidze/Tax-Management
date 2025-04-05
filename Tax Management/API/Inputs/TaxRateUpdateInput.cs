namespace TaxManagement.API.Inputs
{
    public class TaxRateUpdateInput
    {
        public string Municipality { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }

        public double? NewRate { get; set; }
    }
}
