using System.Collections.Generic;
using System.Linq;
using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        // AsNoTracking desabilita o rastreamento, melhorando desempenho
        // Usar quando é garantido de ser uma consulta somente leitura
        // return _context.Categorias.AsNoTracking().ToList();

        // Exemplo tratamento de erro try-catch - meio paia
        try
        {
            throw new DataMisalignedException();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "problema!");
        }
    }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        // Include: carrega entidades relacionadas
        //      Vai incluir as categorias e os produtos!
        return _context.Categorias.Include(p => p.Produtos).ToList();
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _context.Categorias?.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound("Categoria não encontrada...");
        }
        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
            return BadRequest();

        _context.Categorias?.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest();
        }
        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();
        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _context.Categorias?.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound("Categoria não encontrada...");
        }
        _context.Categorias?.Remove(categoria);
        _context.SaveChanges();
        return Ok(categoria);
    }
}