using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Infrastructure.Extensions;
using SocialMedia.Infrastructure.Filters;
using System.Reflection;
using System.Text;

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

builder.Services.AddSwaggerConfig($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
builder.Services.AddServicesConfig(); // Injection services FROM Extensions
builder.Services.AddOptions(builder.Configuration); //AddOptions FROM Extensions
builder.Services.AddDbContexts(builder.Configuration); //AddDbContexts FROM Extensions

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            ValidIssuer  = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    });

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Social Media API");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

//Autorizacion de solicitudes del cliente
app.UseCors();
app.UseCors(builder =>
    builder.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();