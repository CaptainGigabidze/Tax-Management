using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
        private Stopwatch _stopwatch;

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
                _logger.Log(LogLevel.Information, "Request started.");
                _stopwatch = Stopwatch.StartNew();

                //get records from database and map them to output model more suitable for user
                var result = _taxRateRepository.GetAllTaxRates();
                var mappedResult = result.Select(x => TaxRateMapper.MapTaxRateOutput(x));

                _stopwatch.Stop();
                _logger.Log(LogLevel.Information, $"Request finished. Took {_stopwatch.ElapsedMilliseconds}");

                //Only return results if there are any
                if (mappedResult != null && mappedResult.Any())
                    return Ok(mappedResult);
                else
                    return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetSpecificTaxRate")]
        public ActionResult<TaxRateOutput> GetTaxRateByMunicipalityAndDate(string municipality, string date)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Request started.");
                _stopwatch = Stopwatch.StartNew();

                DateTime inputDate;

                //Validate if input data is not empty and provided date is valid
                if (string.IsNullOrEmpty(municipality) || string.IsNullOrEmpty(date) && DateTime.TryParse(date, out inputDate))
                {
                    _stopwatch.Stop();
                    _logger.Log(LogLevel.Warning, $"Incorrect input data. Took {_stopwatch.ElapsedMilliseconds}");
                    return BadRequest();
                }

                //Get specific record from input date and municipality
                var result = _taxRateRepository.GetTaxRateForMunicipalityAndDate(municipality, DateTime.Parse(date));

                //If record exists, map it to proper output model and return
                if (result != null)
                {
                    var mappedResult = TaxRateMapper.MapTaxRateOutput(result);

                    _stopwatch.Stop();
                    _logger.Log(LogLevel.Information, $"Request finished. Took {_stopwatch.ElapsedMilliseconds}");

                    return Ok(mappedResult);
                }


                //If no record exists, return no content
                _stopwatch.Stop();
                _logger.Log(LogLevel.Information, "Requested record does not exist");
                _logger.Log(LogLevel.Information, $"Request finished. Took {_stopwatch.ElapsedMilliseconds}");

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("AddTaxRate")]
        public ActionResult<TaxRateOutput> AddTaxRate([FromQuery] TaxRateInput input)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Request started.");
                _stopwatch = Stopwatch.StartNew();

                //Find if record for selected municipality, date and type already exists
                var existingRate = _taxRateRepository.GetSpecificTaxRate(input.Municipality, DateTime.Parse(input.StartDate), Enum.Parse<TaxRateType>(input.Type));
                if (existingRate != null)
                {
                    _stopwatch.Stop();
                    _logger.Log(LogLevel.Warning, $"Tax record already exists. Took {_stopwatch.ElapsedMilliseconds}");
                    return Conflict();
                }

                //If not record exists, we create new record
                var newRate = TaxRateMapper.MapTaxRateFromInput(input);
                _taxRateRepository.AddTaxRate(newRate);

                _stopwatch.Stop();
                _logger.Log(LogLevel.Information, $"Request finished. Took {_stopwatch.ElapsedMilliseconds}");

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("UpdateTaxRate")]
        public ActionResult<TaxRateOutput> UpdateTaxRecord([FromQuery] TaxRateUpdateInput input)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Request started.");
                _stopwatch = Stopwatch.StartNew();

                //Find existing tax record to update it
                var existingRate = _taxRateRepository.GetSpecificTaxRate(input.Municipality, DateTime.Parse(input.Date), Enum.Parse<TaxRateType>(input.Type));
                if (existingRate != null)
                {
                    _taxRateRepository.UpdateTaxRate(existingRate.Id, input.NewRate);

                    _stopwatch.Stop();
                    _logger.Log(LogLevel.Information, $"Request finished. Took {_stopwatch.ElapsedMilliseconds}");

                    return Ok();
                }
                //If record doesn't exits, return conflict
                else
                {
                    _stopwatch.Stop();
                    _logger.Log(LogLevel.Warning, $"Tax record does not exist. Took {_stopwatch.ElapsedMilliseconds}");
                    return Conflict();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
