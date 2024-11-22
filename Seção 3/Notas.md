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