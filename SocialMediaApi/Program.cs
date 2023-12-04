using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    //options.SuppressModelStateInvalidFilter = true; //Controlar las validaciones manualmente sin afectar las por defecto.
});

//Injection dependencies
builder.Services.AddTransient<IPostRepository, PostRepository>();

//Conexion bd
builder.Services.AddDbContext<SocialMediaContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("stringCnx"))
.EnableSensitiveDataLogging());

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Filtro global
//Metodo Obsoleto
builder.Services.AddMvc(options =>
{
    options.Filters.Add<ValidationFilter>();
}).AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});
//builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddFluentValidationClientsideAdapters();
//builder.Services.AddValidatorsFromAssemblyContaining<ValidationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
