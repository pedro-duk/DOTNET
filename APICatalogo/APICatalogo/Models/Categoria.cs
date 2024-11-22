using System.Collections.ObjectModel;

namespace APICatalogo.Models; // scoped namespaces remove necessidade de abrir/fechar chaves
public class Categoria
{
    public Categoria()
    {
        // Boa prática: sempre inicializar coleções
        Produtos = []; // igual a new Collection<Produto>()
    }
    // prop + tabtab cria propriedades rápido
    // Com nome da entidade + Id no nome, entity framework entende que é chave primária!
    public int CategoriaId { get; set; }

    // Propriedades devem ser nullable, portanto é necessário o "?" em tipos por referência, como strings
    public string? Nome { get; set; }
    public string? ImagemUrl { get; set; }

    // Relacionamento um para muitos - CategoriaId é chave estrangeira em Produtos
    // Pelas convenções, o mínimo necessário para definir um relacionamento é incluir uma 
    //      propriedade de navegação na entidade principal - categoria

    // Propriedade de navegação Produtos
    // Categoria contém uma coleção de produtos - relacionamento 1:n
    //      isso já é suficiente EFCore gera uma chave estrangeira "CategoriaId" em Produtos
    public ICollection<Produto>? Produtos { get; set; }
}
