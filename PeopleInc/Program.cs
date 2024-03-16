using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PeopleInc.Data;
using PeopleInc.Models;
using PeopleInc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
builder.Services.AddDbContext<PeopleIncContext>(opts => opts.UseLazyLoadingProxies().UseSqlite(connectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddScoped<ICRUDService<Pessoa>, PessoasService>();
builder.Services.AddScoped<ICSVService, CSVService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
