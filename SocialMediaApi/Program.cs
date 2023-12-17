using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); //Filtro de excepciones globlal
})
    .AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
})
    .ConfigureApiBehaviorOptions(options =>
{
    //options.SuppressModelStateInvalidFilter = true; //Controlar las validaciones manualmente sin afectar las por defecto.
});

//Injection dependencies
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IUriService>(provider =>
{
    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
    var request = accesor.HttpContext.Request;
    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(absoluteUri);
});

//Para acceder a las variables del settings.json
builder.Services.Configure<PaginationOptions>(builder.Configuration.GetSection("Pagination"));

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
})
    .AddFluentValidation(options =>
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