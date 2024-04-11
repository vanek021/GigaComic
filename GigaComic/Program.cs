using System.Text;
using GigaComic.Authorization;
using GigaComic.Configurations;
using GigaComic.Core.Entities;
using GigaComic.Data;
using GigaComic.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options => {});

builder.Services.AddIdentity<User, IdentityRole>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = AuthConfiguration.AUDIENCE,
            ValidIssuer = AuthConfiguration.ISSUER,
            IssuerSigningKey = AuthConfiguration.GetSymmetricSecurityKey()
        };
    }).AddScheme<AuthenticationSchemeOptions, AuthHandler>(AuthConfiguration.SessionScheme, _ => { });

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
