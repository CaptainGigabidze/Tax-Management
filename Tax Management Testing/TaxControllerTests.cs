using FluentAssertions;
using System.Net.Http.Json;
using TaxManagement.API.Outputs;

namespace Tax_Management_Testing
{
    public class TaxControllerTests
    {
        [Fact]
        public async Task GetTaxRates()
        {
            var application = new TaxManagementApplicationFactory();

            var client = application.CreateClient();

            var response = await client.GetAsync("api/Tax/GetTaxRate");

            response.EnsureSuccessStatusCode();
            var resultResponse = await response.Content.ReadFromJsonAsync<IEnumerable<TaxRateOutput>>();
            resultResponse.Should().NotBeNull();
            resultResponse?.Count().Should().BeGreaterThan(0);
            response.Dispose();
        }

        [Fact]
        public async Task GetSpecificTaxRates()
        {
            var application = new TaxManagementApplicationFactory();

            var additionalInput = "?municipality=Copenhagen&date=01.01.2024";

            var client = application.CreateClient();

            var response = await client.GetAsync("api/Tax/GetSpecificTaxRate" + additionalInput);

            response.EnsureSuccessStatusCode();
            if(response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                var resultResponse = await response.Content.ReadFromJsonAsync<TaxRateOutput>();
                resultResponse.Should().NotBeNull();
            }
            response.Dispose();
        }

        [Fact]
        public async Task AddTaxRate()
        {
            var application = new TaxManagementApplicationFactory();

            var additionalInput = "?Municipality=Odessa&StartDate=01.01.2025&EndDate=31.12.2025&Rate=0.8&Type=Yearly";

            var client = application.CreateClient();

            var response = await client.PostAsync("api/Tax/AddTaxRate" + additionalInput, null);

            response.EnsureSuccessStatusCode();
            response.Dispose();
        }

        [Fact]
        public async Task UpdateTaxRate()
        {
            var application = new TaxManagementApplicationFactory();

            var additionalInput = "?Municipality=Kiyv&Date=01.01.2024&Type=Yearly&NewRate=0.7";

            var client = application.CreateClient();

            var response = await client.PostAsync("api/Tax/UpdateTaxRate" + additionalInput, null);

            response.EnsureSuccessStatusCode();
            response.Dispose();
        }
    }
}