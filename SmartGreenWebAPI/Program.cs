using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SmartGreenAPI.Data;
using SmartGreenAPI.Data.Services;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<UserServices>();

builder.Services.AddScoped<InvernaderoServices>();

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
