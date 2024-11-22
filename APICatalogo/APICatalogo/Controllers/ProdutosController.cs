using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // Instâncias do contexto do EF Core são usadas para rastrear as entidades recuperadas do BD
        // Cada operação usa uma instância de contexto diferente.
        //      Um Get não vai usar uma mesma instância de um Update.
        //      Cenário Desconectado - Métodos assíncronos seriam recomendados.
        //          porém vamos criar síncronos por agora.

        // IEnumerable: Coleção de objetos. Mais otimizado que List nesse caso. 3 motivos principais de usá-lo:
        //      1) Interface é somente leitura
        //      2) Permite adiar execução
        //      3) Permite não ter toda coleção na memória

        // Com o ActionResult, podemos retornar IEnumerable ou qualquer outro retorno suportado em ActionResult, como NotFound.

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            // Conseguimos acessar aqui Produtos em _context por conta do mapeamento em dbcontext
            var produtos = _context.Produtos?.ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")] // Rota nomeada: permite referenciar em outras partes do código
        public ActionResult<Produto> Get(int id)
        {
            // First: Se não encontrar, lança exceção
            // FirstOrDefault: Se não encontrar, vai retornar um null
            var produto = _context.Produtos?.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado");
            }

            return produto;
        }

        [HttpPost]
        // Só ActionResult: só retorna respostas padrão de actionResult
        public ActionResult Post(Produto produto)
        {
            if (produto is null) return BadRequest();

            // Estamos no modo desconectado, precisamos de fazer em 2 passos:
            // 1) incluir informação no contexto (in-memory)
            _context.Produtos?.Add(produto);

            // 2) persistir dados no bd
            _context.SaveChanges();

            // Poderiamos usar Created(), porém CreatedAtRouteResult tem vantagems:
            //      - Informar a uri do recurso no cabeçalho "location" da resposta
            //      - Formata automaticamente o corpo da resposta
            //
            // 1o param (routeName): "ObterProduto" é a URI para obter o recurso que acabou de ser adicionado
            // 2o param (routeValues): Objeto para gerar url 
            // 3o param (value): retorno do body
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            //return Created("ObterProduto", new { id = produto.ProdutoId });
        }

        /*Antes da 2.2 ficaria assim:
        public ActionResult Post([FromBody] Produto produto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        }

        Agora porém, a validação do modelo e a inferência da informação dos dados do body do request é feita automaticamente!
         */


        [HttpPut("{id:int}")] //definir parametro de rota
        public ActionResult Put(int id, Produto produto)
        {
            // Atualização completa
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            // como estamos no modo desconectado, contexto deve ser informado que produto está num estado modificado
            //      para que possa alterar. Fazemos isso com entry
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos?.FirstOrDefault(p => p.ProdutoId == id);
            // var produto = _context.Produtos.Find(id); // Tenta achar na memória antes (id temq ser chave primária na tabela)

            if (produto is null)
            {
                return NotFound("Produto não localizado.");
            }

            _context.Produtos?.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
