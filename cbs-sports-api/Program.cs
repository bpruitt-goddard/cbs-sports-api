using System.Text.Json.Serialization;
using cbs_sports_api.Data;
using cbs_sports_api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Wire up database
// Use in-memory for simplicity, but will be replaced with real database on non-local
builder.Services.AddDbContext<PlayerDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddHttpClient<ICbsApiService, CbsApiService>(client =>
    {
        // This can come from appsettings instead
        client.BaseAddress = new Uri("http://api.cbssports.com/fantasy/"); 
    });

builder.Services.AddScoped<IImportPlayerService, ImportPlayerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
