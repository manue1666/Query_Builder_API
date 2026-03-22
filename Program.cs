using Microsoft.EntityFrameworkCore;
using QueryBuilderApi.Data;
using QueryBuilderApi.Services;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<QueryService>();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Activate Controllers
app.MapControllers();


app.Run();

