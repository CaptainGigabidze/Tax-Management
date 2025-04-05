using TaxManagement.DAL.Models;

namespace TaxManagement.DAL.Repositories
{
    public interface ITaxRateRepository
    {
        IEnumerable<TaxRate> GetAllTaxRates();
        TaxRate GetTaxRateForMunicipalityAndDate(string municipality, DateTime date);
        TaxRate GetSpecificTaxRate(string municipality, DateTime date, TaxRateType type);
        void AddTaxRate(TaxRate taxRate);
        void UpdateTaxRate(int id, double? rate);
    }
}
