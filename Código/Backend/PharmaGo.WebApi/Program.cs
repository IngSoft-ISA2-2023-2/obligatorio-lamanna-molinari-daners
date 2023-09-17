using PharmaGo.Factory;
using PharmaGo.WebApi.Filters;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterBusinessLogicServices(builder.Configuration);
builder.Services.RegisterDataAccessServices(builder.Configuration);
builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowedOrigins",
        policy =>
        {
            policy.WithOrigins("*") // note the port is included 
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyAllowedOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }
