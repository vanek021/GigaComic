using GigaComic.Authorization;
using GigaComic.Configurations;
using GigaComic.Core.Extensions;
using GigaComic.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

var secretKey = builder.Configuration["JWT:SECRET_KEY"];
var authConfiguration = new AuthConfiguration(secretKey);
builder.Services.AddSingleton(authConfiguration);

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);
builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));
builder.Services.AddApplicationIdentity<AppDbContext>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = authConfiguration.SessionScheme;
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
            ValidAudience = authConfiguration.Audience,
            ValidIssuer = authConfiguration.Issuer,
            IssuerSigningKey = authConfiguration.GetSymmetricSecurityKey()
        };
    }).AddScheme<AuthenticationSchemeOptions, AuthHandler>(authConfiguration.SessionScheme, _ => { });

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
