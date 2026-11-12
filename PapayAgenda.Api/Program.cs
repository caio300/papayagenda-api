using Microsoft.EntityFrameworkCore;
using PapayAgenda.Infrastructure.Data;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// String de conexão
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext na API com MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
builder.Services.AddScoped<IServicoService, ServicoService>();
builder.Services.AddScoped<ICategoriaServicoService, CategoriaServicoService>();
builder.Services.AddScoped<IAgendamentoService, AgendamentoService>();

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