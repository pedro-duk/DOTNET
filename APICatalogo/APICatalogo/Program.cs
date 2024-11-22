using System.Text.Json.Serialization;
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => 
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar serviços e configurar dbcontext
var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Incluir serviço do contexto do EF Core no container DI Nativo
// Permite que injetemos instância do AppDbContext aonde precisarmos dela (nos repos)
//      Tipo T define contexto
//      informar provedor e string de conexão na função lambda
builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection,
                    ServerVersion.AutoDetect(mySqlConnection)));

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
