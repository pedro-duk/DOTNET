// Cria um construtor de aplicativos
var builder = WebApplication.CreateBuilder(args);

// Inicialização
// Registro dos serviços no container DI
// Configuração do pipeline do request

// Add services to the container --> Equivale ao ConfigureServices() do Startup

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline. --> Equivale ao Configure()
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
