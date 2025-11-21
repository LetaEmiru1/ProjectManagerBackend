using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Data;
using ProjectManagerApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers (The Face)
builder.Services.AddControllers();

// 2. Add Swagger (The Documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Add Database (The Memory) - Reads from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(connectionString));

// 4. Add Service (The Brain)
// CRITICAL: This must be Scoped because the Database is Scoped.
builder.Services.AddScoped<ProjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();