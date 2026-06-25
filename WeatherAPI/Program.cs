using WeatherAPI.Configurations;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Config binding
builder.Services.Configure<WeatherApiSettings>(
    builder.Configuration.GetSection("WeatherApi"));

// HttpClient
builder.Services.AddHttpClient();

builder.Services.AddMemoryCache(); //IMemoryCache
builder.Services.AddScoped<WeatherService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
