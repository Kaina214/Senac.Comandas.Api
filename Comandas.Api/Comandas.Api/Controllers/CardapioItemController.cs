using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    //CRIAR ROTA DO CONTROLADOR
    [Route("api/[controller]")]
    [ApiController]//Define que essa classe é um controlador de API
    public class CardapioItemController : ControllerBase
    {
        public ComandasDbContext _context { get; set; }

        public CardapioItemController(ComandasDbContext context)
        {
            _context = context;
        }
      
        [HttpGet]
        public IResult Get()
        {
            var cardapios = _context.CardapioItens.ToList();
            return Results.Ok(cardapios);
        }

        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var cardapio = _context.CardapioItens.  
                FirstOrDefault(c => c.Id == id);
            if (cardapio is null)
            {
                return Results.NotFound("Cárdapio não encontrado!");
            }
            return Results.Ok(cardapio);
        }

        [HttpPost]
        public IResult Post([FromBody] CardapioItemCreateRequest cardapio)//post adicionar
        {
            if (cardapio.Titulo.Length < 3)
                return Results.BadRequest("O título deve ter no mínimo 3 caracteres.");
            if (cardapio.Descricao.Length < 3)
                return Results.BadRequest("A descrição deve ter no mínimo 3 caracteres.");
            if (cardapio.Preco <= 0)
                return Results.BadRequest("O preço deve ser maior que zero.");

            var cardapioItem = new CardapioItem
            {
              
                Titulo = cardapio.Titulo,
                Descricao = cardapio.Descricao,
                Preco = cardapio.Preco,
                PossuiPreparo = cardapio.PossuiPreparo
            };

            _context.CardapioItens.Add(cardapioItem); // Ensure this is inside the method body

            _context.SaveChanges();

            return Results.Created($"/api/cardapioitem/{cardapioItem.Id}", cardapioItem);

           
        }

        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] CardapioItemUpdateRequest cardapio)//put atualiza
        {
            var cardapioItem = _context.CardapioItens.
                FirstOrDefault(c => c.Id == id);
            if (cardapioItem is null)
                return Results.NotFound($"Cardápio {id} não encontrado!");

            cardapioItem.Titulo = cardapio.Titulo;
            cardapioItem.Descricao = cardapio.Descricao;
            cardapioItem.Preco = cardapio.Preco;
            cardapioItem.PossuiPreparo = cardapio.PossuiPreparo;

            _context.SaveChanges();
            return Results.NoContent();
        }
       

        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            //buscar o cardapio da lista pelo id
            var cardapioItem = _context.CardapioItens
                .FirstOrDefault(c => c.Id == id);
            // se tiver nulo, retorna 404
            if (cardapioItem is null)
                return Results.NotFound($"Cardápio {id} não encontrado!");
            //remove o objeto cardapio da lista
            _context.CardapioItens.Remove(cardapioItem);
            var removido = _context.SaveChanges();
            //retornar 204 sem conteudo
            if (removido > 0)
            {
                return Results.NoContent();
            }

            return Results.StatusCode(500);
        }
    }
}
