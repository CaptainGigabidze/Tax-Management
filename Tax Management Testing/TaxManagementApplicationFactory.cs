using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Tax_Management_Testing
{
    internal class TaxManagementApplicationFactory : WebApplicationFactory<Program>
    {
       
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //Force tests to use testing environment so it uses testing database
            builder.UseEnvironment("Testing");
        }
    }
}
