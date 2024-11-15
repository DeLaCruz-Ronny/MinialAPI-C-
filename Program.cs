

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using minimalAPIPeliculas;
using minimalAPIPeliculas.Endpoints;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Repositorios;
using minimalAPIPeliculas.Servicios;
using minimalAPIPeliculas.Utilidades;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

//Inicio del area de los servicios

//Conexion
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("Cadena")));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

//CORS
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(conf =>
    {
        conf.WithMethods(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy("libre", conf =>
    {
        conf.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//CACHE
builder.Services.AddOutputCache();

//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
builder.Services.AddScoped<IRepositorioPeliculas, RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArichivosAzure>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(Program));

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddAuthentication().AddJwtBearer(opciones => 
opciones.TokenValidationParameters = new TokenValidationParameters 
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    //IssuerSigningKey = Llaves.ObtenerTodasLlave(builder.Configuration).First(),
    IssuerSigningKeys = Llaves.ObtenerTodasLlave(builder.Configuration),
    ClockSkew = TimeSpan.Zero
});

builder.Services.AddAuthorization();

//Fin del area de los servicios

var app = builder.Build();

//Inicio Middleware
// if (builder.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseOutputCache();

app.UseAuthorization();

//app.MapGet("/", [EnableCors("libre")] () => "Hello World!");

//Crea un grupo de endpoints para evitar codigo repetitivo
app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();


//Fin Middleware
app.Run();

