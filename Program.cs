var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string path = Path.Combine(AppContext.BaseDirectory, "pasajeros.txt");

// Inyeccion de dependencias
builder.Services.AddScoped<IPasajeroRepository>(provider => new PasajeroRepositoryArchivo(path));
builder.Services.AddScoped<PasajeroService>();
builder.Services.AddScoped<CheckInService>();
builder.Services.AddScoped<EquipajeService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
