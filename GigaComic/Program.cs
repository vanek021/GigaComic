using GigaComic.Core.Extensions;
using GigaComic.Data;
using GigaComic.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);
builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));
builder.Services.AddApplicationIdentity<AppDbContext>();

builder.AddGigaComicAuthentication();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAppServices();

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
