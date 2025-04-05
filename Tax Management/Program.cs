using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Linq.Expressions;
using System.Reflection;
using TaxManagement.DAL.Context;
using TaxManagement.DAL.Repositories;


var builder = WebApplication.CreateBuilder(args);

//Setup settings depending on environment, needed to use different databases
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath).AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json").AddEnvironmentVariables();

Console.WriteLine(builder.Environment.EnvironmentName);
//Registed database context and repository service
builder.Services.AddDbContext<TaxManagementContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TaxManagementDatabase")), ServiceLifetime.Singleton);
builder.Services.AddScoped<ITaxRateRepository, TaxRateRepository>();

//Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//Add MVC and add controllers
builder.Services.AddMvc(options => options.EnableEndpointRouting = false).AddApplicationPart(Assembly.Load("Tax Management")).AddControllersAsServices();

//Add SwaggerGen to document API
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tax Management"
    });
});

var app = builder.Build();

//Setup routes for API
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "api/{controller}/{action=Index}/{id?}");
});

//Added Swagger to utilize API via interface instead of writing uri
app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Tax Management v1");
    opt.RoutePrefix = "taxmanagement/swagger";
});

//Migrate database on application start to ensure it's up to date with current model
var db = app.Services.GetRequiredService<TaxManagementContext>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
try
{
    logger.Log(LogLevel.Information, "Starting applying EF migrations.");
    db.Database.Migrate();
}
catch(Exception ex) 
{ 
    logger.LogError(ex.Message); 
}


app.Run();
