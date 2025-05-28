using AutoMapper;
using FinancialApi.Models;
using FinancialApi.Profiles;
using FinancialApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// In-memory data
var departments = new List<Department>
{
    new Department { Id = 1, Name = "IT", BudgetLimit = 50000 },
    new Department { Id = 2, Name = "Marketing", BudgetLimit = 30000 }
};
var transactions = new List<Transaction>();

// Register services with in-memory data
builder.Services.AddSingleton(departments);
builder.Services.AddSingleton(transactions);
builder.Services.AddScoped<IReportService, ReportService>(provider =>
{
    var deps = provider.GetRequiredService<List<Department>>();
    var trans = provider.GetRequiredService<List<Transaction>>();
    return new ReportService(trans, deps);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
