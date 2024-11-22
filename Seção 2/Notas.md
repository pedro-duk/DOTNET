## Arquivo Program.cs

Serve para:

- Inicialização
- Registro dos serviços no container DI
- Configuração do pipeline do request

## Controller

Convenção de nomenclatura

- Sufixo Controller
- Prefixo que identifica o controlador é usado como parte da rota base definida pelo atributo Route
  - No caso abaixo, está "[controller]", o que indica que a rota base será substituída pelo prefixo "WeatherForecast"

```c#
using Microsoft.AspNetCore.Mvc;

namespace MinhaAPI.Controllers
{
	[ApiController] // Oferece diversos recursos e distingue de um controlador MVC.
	// Exemplos de recursos: convenções de roteamento, binding automatico, validação automática, resposta no formato padrão, documentação automática etc.
	[Route("[controller]")] // Rota base: WeatherForecast
	public class WeatherForecastController : ControllerBase // Lida com requisições http, omitindo funcionalidades de suporte a Views (não existem em controladores)
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;

		// Injeção de dependência no construtor
		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		// Cria endpoint com action HTTP GET
		// Parâmetro Name opcional para a rota.
		// Como é opcional, oq vale é a rota base.
		// Sem Name, a rota seria WeatherForecast/GetWeatherForecast
		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
		}
	}
}
```

## Minimal APIs

Abordagem para criação de APIs simples e concisas sem controladores.

- Endpoints definidos no código da aplicação, no Program.cs
  - Métodos MapGet, MapPost, etc disponíveis no objeto IApplicationBuilder

```C#
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Define uma função lambda
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

// Classe WeatherForecast substituída por um record
// é um tipo de classe otimizado para representar dados imutáveis de forma simples
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


```

## Para criar APIs com VSCODE e .NET CLI

- Criar projetos com .NET CLI, usar VSCode como editor

- Todos comandos: https://learn.microsoft.com/en-us/dotnet/core/tools

Comandos mais usados:

- `dotnet help`
- `dotnet new webapi --o DemoApi`: Cria uma pasta "DemoApi" com um projeto "DemoApi"
- `dotnet new webapi -minimal -o DemoMinimalApi`: Cria um Minimal API
- `code .`: abre o projeto no vscode
- `dotnet run`

- Outra opção: Usar .NET new project do VSCODE, no plugin C# DEV KIT

## Persistência dos dados - Definições do nosso projeto

- **Banco de dados relacional**: MySql
- **Provedor de banco de dados**: Pomelo.EntityFrameworkCore.MySql
- **Tecnologia para tratar os dados**: Entity Framework Core
- **Abordagem**: Code-first - Parte das entidades para criar as tabelas e o banco de dados
- **Outros**: Padrão Repositório - Desacopla o acesso aos dados da aplicação

## Padrões de nomes

- Nome do Projeto: Não tem padrão - CategoriaApi
- Nome dos Controllers: No plural - ProdutosController, CategoriasController...
- Nome das Actions: Verbos http - HttpGet, HttpPost
- Instância de um recurso: No plural - /v1/api/ProdutosController
- Usar substantivos e não verbos: /v1/api/produtos e não /v1/api/GetTodosProdutos

## Estrutura do projeto

Arquitetura em camadas

- Apresentação: UI
- Negócios: coordena aplicação, toma decisões lógicas
- Acesso a dados

**Layers**: Separação lógica - Organização do código e dos dados
**Tiers**: Separação física - Distribuição do código e dos dados

n-layer não implica n-tier, e vice-versa

Usaremos separação lógica do projeto usando pastas no mesmo projeto. Criar projetos distintos somente em casos muito complexos.

**Estrutura nossa**:
1) Presentation Layer - Angular/views/Mobile/Desktop
2) Service Layer
3) Business Logic/Application Core - Repositorios,domínio
4) Data Access/Persistence - EF Core, MYsql

# Criando o projeto WebApi

