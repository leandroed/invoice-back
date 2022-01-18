using InvoiceApi;
using InvoiceApi.Data;
using InvoiceApi.Services;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

LoggerConfig.InitializeLogger();
bool.TryParse(builder.Configuration["CloudEnv"], out bool isCloudEnv);
DatabaseConfig dbConfig = new DatabaseConfig(isCloudEnv);

if (!dbConfig.StartConnection())
{
    Log.Error("Error it was not possible connect with database.");
    return;
}

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "InvoiceApi",
        Description = "An ASP.NET Core Web API for managing Invoice items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license"),
        },
    });
});

builder.Services.AddScoped<InvoiceExternalData>();
builder.Services.AddScoped<SalesRepository>();
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
