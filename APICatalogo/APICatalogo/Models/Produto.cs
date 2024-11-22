using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;
public class Produto
{
    // Classes anêmicas: possuem somente propriedades
    [Key]
    public int ProdutoId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    // as propriedades abaixo não são necessárias para criar a chave estrangeira,
    //      mas são boas para explicitar a relação e ter propriedades de navegação
    // Propriedade mapeada para chave-estrangeira no banco de dados
    public int CategoriaId { get; set; }

    // Propriedade de navegação - Indica que um produto está relacionado com uma Categoria.
    public Categoria? Categoria { get; set; }
}
