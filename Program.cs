

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using minimalAPIPeliculas;
using minimalAPIPeliculas.Entidades;
using minimalAPIPeliculas.Repositorios;
var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

//Inicio del area de los servicios

//Conexion
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("Cadena")));

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

//app.MapGet("/", [EnableCors("libre")] () => "Hello World!");

app.MapGet("/generos", async (IRepositorioGeneros repo) =>
{
    return await repo.ObtenerTodos();
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos_get"));

app.MapGet("/generos/{id:int}", async (IRepositorioGeneros repo, int id) => {
    var genero = await repo.ObtenerPorId(id);
    if (genero is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(genero);
});

app.MapPost("/generos", async (Genero genero, IRepositorioGeneros repo, IOutputCacheStore output) => {
    var id = await repo.Crear(genero);
    await output.EvictByTagAsync("generos_get", default); //Eliminar cache despues de agregar un registro
    return Results.Created($"/generos/{id}",genero);
});

//Fin Middleware
app.Run();
