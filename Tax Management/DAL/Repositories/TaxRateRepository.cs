using System.Runtime.CompilerServices;
using TaxManagement.DAL.Context;
using TaxManagement.DAL.Models;

namespace TaxManagement.DAL.Repositories
{
    public class TaxRateRepository : ITaxRateRepository
    {
        private readonly TaxManagementContext _context;
        private readonly ILogger<TaxRateRepository> _logger;

        public TaxRateRepository(TaxManagementContext context, ILogger<TaxRateRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<TaxRate> GetAllTaxRates()
        {
            return _context.TaxRates;
        }

        public TaxRate GetTaxRateForMunicipalityAndDate(string municipality, DateTime date)
        {
            var matchingRates = _context.TaxRates.Where(x => x.Municipality == municipality && x.StartDate <= date && x.EndDate >= date);
            if(matchingRates != null && matchingRates.Any())
            {
                var minRateType = matchingRates.Min(x => x.Type);
                return matchingRates.Single(x => x.Type == minRateType);
            }
            return null;
        }

        public TaxRate GetSpecificTaxRate(string municipality, DateTime date, TaxRateType type)
        {
            return _context.TaxRates.SingleOrDefault(x => x.Municipality == municipality && x.StartDate <= date && x.EndDate >= date && x.Type == type);
        }

        public void AddTaxRate(TaxRate taxRate)
        {
            _context.TaxRates.Add(taxRate);
            _context.SaveChanges();
        }

        public void UpdateTaxRate(int id, double? rate)
        {
            var entryToUpdate = _context.TaxRates.Single(x => x.Id == id);
            if(rate.HasValue)
            {
                entryToUpdate.Rate = rate.Value;
                _context.SaveChanges();
            } 
        }
    }
}
