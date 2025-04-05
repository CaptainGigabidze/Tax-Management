using TaxManagement.API.Inputs;
using TaxManagement.API.Outputs;
using TaxManagement.DAL.Models;

namespace TaxManagement.API.Mapping
{
    public class TaxRateMapper
    {
        public static TaxRateOutput MapTaxRateOutput(TaxRate taxRate)
        {
            return new TaxRateOutput()
            {
                Municipality = taxRate.Municipality,
                StartDate = taxRate.StartDate.Date.ToShortDateString(),
                EndDate = taxRate.EndDate.Date.ToShortDateString(),
                Rate = taxRate.Rate,
                Type = taxRate.Type.ToString()
            };
        }

        public static TaxRate MapTaxRateFromInput(TaxRateInput input)
        {
            return new TaxRate()
            {
                Municipality = input.Municipality,
                StartDate = DateTime.Parse(input.StartDate),
                EndDate = DateTime.Parse(input.EndDate),
                Rate = input.Rate,
                Type = Enum.Parse<TaxRateType>(input.Type)
            };
        }
    }
}
