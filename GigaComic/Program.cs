using GigaComic.Core.Extensions;
using GigaComic.Data;
using GigaComic.Extensions;
using GigaComic.Modules.Kandinsky;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);
builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));
builder.Services.AddApplicationIdentity<AppDbContext>();

builder.AddGigaComicAuthentication();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
});

builder.Services.AddHttpClient<KandinskyApi>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["Kandinsky:BaseUri"]);
    options.DefaultRequestHeaders.Add("X-Key", builder.Configuration["Kandinsky:Key"]);
    options.DefaultRequestHeaders.Add("X-Secret", builder.Configuration["Kandinsky:Secret"]);
});

builder.Services.AddControllers();
builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GigaGomic"));

app.Run();
