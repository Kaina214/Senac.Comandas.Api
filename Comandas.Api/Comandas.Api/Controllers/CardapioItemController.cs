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
        static List<CardapioItem> cardapios = new List<CardapioItem>() // Ensure this is a field, not a local variable
        {
            new CardapioItem
            {
                Id = 1,
                Titulo = "Coxinha",
                Descricao = "Coxinha de frango com catupiry",
                Preco = 5.50m,
                PossuiPreparo = true
            },
            new CardapioItem
            {
                Id = 2,
                Titulo = "X-Salada",
                Descricao = "X-Salada",
                Preco = 25.50m,
                PossuiPreparo = true
            }
        };

        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(cardapios);
        }

        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var cardapio = cardapios.FirstOrDefault(c => c.Id == id);
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
                Id = cardapios.Count + 1,
                Titulo = cardapio.Titulo,
                Descricao = cardapio.Descricao,
                Preco = cardapio.Preco,
                PossuiPreparo = cardapio.PossuiPreparo
            };

            cardapios.Add(cardapioItem); // Ensure this is inside the method body
            return Results.Created($"/api/cardapioitem/{cardapioItem.Id}", cardapioItem);
        }

        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] CardapioItemUpdateRequest cardapio)//put atualiza
        {
            var cardapioItem = cardapios.FirstOrDefault(c => c.Id == id);
            if (cardapioItem is null)
                return Results.NotFound($"Cardápio {id} não encontrado!");

            cardapioItem.Titulo = cardapio.Titulo;
            cardapioItem.Descricao = cardapio.Descricao;
            cardapioItem.Preco = cardapio.Preco;
            cardapioItem.PossuiPreparo = cardapio.PossuiPreparo;

            return Results.NoContent();
        }
       

        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            //buscar o cardapio da lista pelo id
            var cardapioItem = cardapios
                .FirstOrDefault(c => c.Id == id);
            // se tiver nulo, retorna 404
            if (cardapioItem is null)
                return Results.NotFound($"Cardápio {id} não encontrado!");
            //remove o objeto cardapio da lista
          var removido =  cardapios.Remove(cardapioItem);
            //retornar 204 sem conteudo
            if(removido)
            return Results.NoContent();

            return Results.StatusCode(500);
        }
    }
}
