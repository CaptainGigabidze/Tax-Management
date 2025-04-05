using Microsoft.AspNetCore.Mvc;
using TaxManagement.API.Inputs;
using TaxManagement.API.Mapping;
using TaxManagement.API.Outputs;
using TaxManagement.DAL.Models;
using TaxManagement.DAL.Repositories;

namespace TaxManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxController : ControllerBase
    {
        private readonly ITaxRateRepository _taxRateRepository;
        private readonly ILogger<TaxController> _logger;

        public TaxController(ITaxRateRepository taxRateRepository, ILogger<TaxController> logger)
        {
            _taxRateRepository = taxRateRepository;
            _logger = logger;
        }

        [HttpGet("GetTaxRate")]
        public  ActionResult<IEnumerable<TaxRateOutput>> GetTaxRates()
        {
            try
            {
                var result = _taxRateRepository.GetAllTaxRates();
                var mappedResult = result.Select(x => TaxRateMapper.MapTaxRateOutput(x));
                if (mappedResult != null && mappedResult.Any())
                    return Ok(mappedResult);
                else
                    return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetSpecificTaxRate")]
        public ActionResult<TaxRateOutput> GetTaxRateByMunicipalityAndDate(string municipality, string date)
        {
            try
            {
                if (string.IsNullOrEmpty(municipality) || string.IsNullOrEmpty(date))
                    return BadRequest();
                var result = _taxRateRepository.GetTaxRateForMunicipalityAndDate(municipality, DateTime.Parse(date));
                if (result != null)
                {
                    var mappedResult = TaxRateMapper.MapTaxRateOutput(result);
                    return Ok(mappedResult);
                }
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("AddTaxRate")]
        public ActionResult<TaxRateOutput> AddTaxRate([FromQuery] TaxRateInput input)
        {
            try
            {
                var existingRate = _taxRateRepository.GetSpecificTaxRate(input.Municipality, DateTime.Parse(input.StartDate), Enum.Parse<TaxRateType>(input.Type));
                if (existingRate != null)
                    return BadRequest();
                var newRate = TaxRateMapper.MapTaxRateFromInput(input);
                _taxRateRepository.AddTaxRate(newRate);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("UpdateTaxRate")]
        public ActionResult<TaxRateOutput> UpdateTaxRecord([FromQuery] TaxRateUpdateInput input)
        {
            try
            {
                var existingRate = _taxRateRepository.GetSpecificTaxRate(input.Municipality, DateTime.Parse(input.Date), Enum.Parse<TaxRateType>(input.Type));
                if (existingRate != null)
                {
                    _taxRateRepository.UpdateTaxRate(existingRate.Id, input.NewRate);
                    return Ok();
                }
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
