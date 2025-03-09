using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartGreenAPI.Data;
using SmartGreenAPI.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmartGreenWebAPI.Filters;
using SmartGreenWebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Agregando autenticación JWTBearer

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;

        string jwtsecret = builder.Configuration["JwtSettings:Secret"]!;

        SymmetricSecurityKey signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsecret));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingkey,
            LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,TokenValidationParameters validationParameters) =>
            {
                return expires.HasValue && expires > DateTime.UtcNow;
            }
        };
    });

builder.Services.AddAuthorization();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoConfiguration>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton((sp =>
{
    var config = sp.GetRequiredService<IOptions<MongoConfiguration>>().Value;
    return new MongoClient(config.Connection);
}));

//builder.Services.AddScoped<ExceptionFilter>();

builder.Services.AddTransient<ExceptionMiddleware>();

builder.Services.AddScoped<UserServices>();

builder.Services.AddScoped<InvernaderoServices>();

builder.Services.AddScoped<InverStatusServices>();

builder.Services.AddScoped<AuthUserService>();

builder.WebHost.UseUrls("http://0.0.0.0:5062");

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowFrontEnd", policy =>
    {
        policy.AllowAnyOrigin() //aqui pones la url de tu frontend
        .AllowAnyHeader()
        .AllowAnyMethod();
    });

});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontEnd");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
