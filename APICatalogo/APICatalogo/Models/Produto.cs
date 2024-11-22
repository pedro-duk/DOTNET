namespace APICatalogo.Models;
public class Produto
{
    // Classes anêmicas: possuem somente propriedades
    public int ProdutoId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
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
