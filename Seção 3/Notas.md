# Usar EF (Entity-framework) Core na abordagem Code-first

Realizar o mapeamento das classes das endidades do domínio e gerar o banco de dados e tabelas a partir delas!

1) Instalar pacotes (referências ao EF Core e ao provedor Pomelo):
  - Tools > NuGet Package Manager Console: `install-package <nome>`
  - NET CLI: `dotnet add package <nome>` (na pasta do arquivo)
  - Tools > Nuget Package Manager > Manage Nuget Packages for solution

2) Instalar ferramenta EF Core Tools (para migrations)
  - `dotnet tool install --global dotnet-ef`
  - `dotnet tool update --global dotnet-ef`
  - `dotnet ef`

2) Criar entidades como classes anêmicas (sem métodos, só propriedades)
- Usa convenções para criar banco de dados a partir das entidades do domínio (recurso MIGRATIONS)

## Classe de Contexto
- Definir mapeamento entre entidades e tabelas
- Uma instância DbContext representa uma sessão com o banco de dados;
- DbContextOption para configurar o contexto do EF Core na classe base(define provedor de banco e string de conexão)
- DbSet: Representa coleção de entidades no contexto q podem ser realizadas operações CRUD
  - Tipo T define qual é a entidade mapeada

`DbSet<Categoria> Categorias`: Indica uma tabela "Categorias" com as colunas correspondendo às propriedades da entidade "Categoria"

## EF Core Migrations
- Recurso Migrations atualiza de forma incremental o esquema de BD para mantê-lo em sicronia com o modelo de dados do aplicativo, preservando os dados existentes.
- Versiona o esquema de BD sincronizado com o modelo do EF Core

  - Modelo do EF Core é criado a partir do Modelo de Entidades (domínio) com base na configuração do arquivo de contexto
  - Depois, aplicando o migrations, o modelo do EF Core é usado para atualizar o banco de dados com base no modelo de entidades
  - Sempre que alterar as classes de modelo de domínio é necessário executar migrations

Para usar migrations: EF Core Tools
- `dotnet ef`: verifica instalação
- `dotnet ef migrations add <nome>`: cria script de migração
- `dotnet ef migrations remove <nome>`: remove script de migração
- `dotnet ef migrations update`: aplica scripts gerando/alterando bd

Também possível usando o package manager console
- `add-migration <nome>`
- `remove-migration <nome>`
- `update-database`

## Data Annotations
- Atributos aplicados a classes e seus membros para fornecer metadados sobre como esses recursos devem ser tratados pelo sistema;
  - Realizar validações de entrada de dados;
  - Influenciar comportamento do modelo de dados.

```
System.ComponentModel.DataAnnotations
System.ComponentModel.DataAnnotations.Schema
```

1) Validação de dados
2) Formatação e exibição de dados
3) Geração de código
4) Especificar o relacionamento entre entidades
5) Sobrescrever convenções do EF Core (por exemplo definindo um tipo especifico no bd)

Atributos:
- Key - especifica como chave primária
- Table(nome) - define o nome da tabela para a qual será mapeada
- Column - define a coluna para a qual será mapeada
- DataType - associa um tipo de dados adicional
- ForeignKey - especifica como chave estrangeira
- NotMapped - exclui do mapeamento
- StringLength - tamanho máximo e mínimo do tipo
- Required - especifica not null

Alternativa mais elegante ao Data Annotations: Fluent API

## Popular tabelas com dados
Diversas maneiras:

- Incluir manualmente com SQL
- Usar método OnModelCreating do arquivo de contexto e definir usando propriedade do HasData do EF Core
- Criar método Estático e definir código para incluir dados usando método AddRange do EF Core
- Criar migração vazia e usar métodos Up() e Down() definindo nestes métodos as instruções SQL (insert into)


## Controllers
[ApiController] decora controladores habilitando recursos como:
- Requisito de roteamento de atributo;
- Validação do model state;
- Inferência de parâmetro bind/source;
- Inferência de solicitação de dados de várias partes/formulário;
- Uso de Problem Details para códigos de status de erro.

[Route] especifica um padrão de url para acessar controller ou action
- `[Route("[Controller]")]` indica que a rota possui mesmo nome do controlador

- `[Route("[Controller]/{action}")]` indica que as rotas vão ser acessadas pelo mesmo nome dos métodos action, exemplo:

```C#
[Route("[controller]/{action}")] // variável
public class TesteController : ControllerBase
{

  [HttpGet]
  public string get1() {
    return "get1"
  }

  [HttpGet]
  public string get2() {
    return "get2"
  }
}

// Para acessar get1: GET /teste/get1
// Para acessar get2: GET /teste/get2
```

## Adicionar controller
Botão direito na pasta controller > Add > Controller 

## Serialização e Desserialização - referência cíclica
Acontece quando "A referencia B" e "B referencia A"
- Numa serialização que A inclua B, o referenceHandler acaba buscando ciclicamente pelas referências.
- ReferenceHandler: define como o JsonSerializer lida com referências sobre serialização e desserialização
- Use ReferenceHandler.IgnoreCycles => Ignora o objeto quando um ciclo de referência é detectado 

## Serialização e Desserialização - Propriedades Públicas
- Todas propriedades públicas são serializadas (até as propriedades de navegação)
- Para evitar isso:
  - Método 1: Ignorar propriedades individuais: `[JsonIgnore]` (sempre ignora) ou `[JsonIgnore(Condition = JsonIgnoreCondition.Always)]` (equivalente)
    - Never (nunca ignora)
    - WhenWritingDefault (ignora quando for valor nullable com null, ou tipo de valor padrão)
    - WhenWritingNull (ignora se for tipo de referência null, ou tipo de valor que pode ser anulado com valor null)
  - Método 2: Ignorar todas (adicionar DefaultIgnoreCondition do JsonSerializerOptions)

## Otimizando o código
Quando consultamos entidades usando o EF Core, ele armazena as entidades no contexto em cache realizando o tracking/rastreamento das entidades para acompanhar o estado das entidades.
- Recurso poderoso mas adiciona sobrecarga que afeta desempenho
- Para melhorar: adicionar AsNoTracking()
  - Usar só em consultas somente leitura. Com consultas não rastreadas, não é possível fazer alterações pois não dá pra saber o estado dos objetos.


Outras dicas
 - Nunca retorne todos registros
 - Nunca retorne objetos relacionados sem aplicar filtro


## Tratamento de erro
### Ambiente de desenvolvimento
Usa por padrão a página de exceção do desenvolvedor, com informações detalhadas sobre exceções:
- StackTrace
- Querystring
- Cookies
- Headers

### Ambiente de produção
Tratamento de erros personalizado com UseExceptionHandler
- Captura e registra exceções não tratadas
- Executa novamente o request em um pipeline alternativo usando o caminho indicado (quando response não for iniciado)
  - Código gerado executa request no caminho /Error

  