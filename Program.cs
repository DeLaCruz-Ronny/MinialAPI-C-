

using Microsoft.AspNetCore.Cors;
using minimalAPIPeliculas.Entidades;
var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

//Inicio del area de los servicios


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

app.MapGet("/", [EnableCors("libre")] () => "Hello World!");

app.MapGet("/generos", () =>
{
    var generos = new List<Genero>
    {
        new Genero {
            Id = 1,
            Nombre = "Accion"
        },
        new Genero {
            Id = 2,
            Nombre = "Drama"
        },
        new Genero {
            Id = 3,
            Nombre = "Comedia"
        }
    };

    return generos;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

//Fin Middleware
app.Run();
