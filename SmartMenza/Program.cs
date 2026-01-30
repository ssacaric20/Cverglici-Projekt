using Microsoft.OpenApi.Models;
using SmartMenza.API.Configuration;
using SmartMenza.Business.DependencyInjection;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SmartMenza API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Unesi token ovako: Bearer {token}"
    });


    // "Lock" samo na endpointima koji imaju [Authorize]
    c.OperationFilter<SmartMenza.API.Configuration.AuthorizeOperationFilter>();
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var storageConn = builder.Configuration["AzureStorage:ConnectionString"];

if (string.IsNullOrWhiteSpace(storageConn))
{
    throw new Exception("AzureStorage:ConnectionString is missing (set AzureStorage__ConnectionString).");
}

builder.Services.AddSingleton(new BlobServiceClient(storageConn));

builder.Services.AddJwtAuthentication();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
