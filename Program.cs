using QueryBuilderApi.Services;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseService>();

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

