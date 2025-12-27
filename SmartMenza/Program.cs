using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMenza.Business.Services;
using SmartMenza.Data.Data;
using System.Text;
using SmartMenza.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IFoodAnalyzer, RuleBasedFoodAnalyzer>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IDailyMenuService, DailyMenuServices>();
builder.Services.AddScoped<IDishService, DishServices>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes("f17ca2eec6a314d8f6d80fddb6bd4135a2a5a5a715700e463f788465a221f099")
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
