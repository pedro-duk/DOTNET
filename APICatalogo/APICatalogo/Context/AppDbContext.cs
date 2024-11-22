using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context;
public class AppDbContext : DbContext
{
    // Realiza mapeamento e comunicação entre entidades e BD relacional
    // ctor tab tab: cria construtor
    // options: configurações para configurar contexto
    // está passando para a classe base DbContext pelo operador "base" (similar a super)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // DbSet: representação de uma tabela no bd, está sendo mapeada para as entidades
    // Nullable garante q propriedades podem ser opcionais
    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }

    // Strings de conexão estão em appsettings.json
}
