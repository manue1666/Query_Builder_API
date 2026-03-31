using Microsoft.EntityFrameworkCore;
using QueryBuilderApi.Data;
using QueryBuilderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Para Railway: convertir DATABASE_URL a ConnectionString
var databaseUrl = builder.Configuration["DATABASE_URL"];
if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var db = uri.AbsolutePath.Substring(1);
    var user = uri.UserInfo.Split(":")[0];
    var password = uri.UserInfo.Split(":")[1];
    var host = uri.Host;
    var port = uri.Port;
    var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password};SSL Mode=Require;";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

//Services
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddScoped<QueryService>();
builder.Services.AddScoped<GroqService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "QueryBuilderAPI",
            ValidateAudience = true,
            ValidAudience = "QueryBuilderClients",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(secretKey ?? "")
            )
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "http://localhost:3000",
                "https://gestion-pacientes-59510.web.app/"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Activate Controllers
app.MapControllers();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log the error
        Console.WriteLine($"Migration failed: {ex.Message}");
    }
}

app.Run();

