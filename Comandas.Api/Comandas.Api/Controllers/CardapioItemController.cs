using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    //CRIAR ROTA DO CONTROLADOR
    [Route("api/[controller]")]
    [ApiController]//Define que essa classe é um controlador de API
    public class CardapioItemController : ControllerBase//Herda de ControllerBase para poder a requisicoes http
    {
        List<CardapioItem> cardapios = new List<CardapioItem>()
        {
            new CardapioItem{
                Id = 1,
                Descricao = "Coxinha de frango com catupiry",
                Preco = 5.50m,
                PossuiPreparo = true
            },
            new CardapioItem {
                Id = 2,
                Descricao = "X-Salada",
                Preco = 25.50m,
                PossuiPreparo = true
            }
            };

        //METODO GET que retorna a lista de cardápio
        // GET: api/<CardapioItemController>
        [HttpGet]
        public IResult Get()
        {
           return Results.Ok(cardapios);
        }
       
        // GET api/<CardapioItemController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            //BUSCAR NA LISTA DE CARDAPIO DE ACORDO COM O ID DO PARAMETRO
            // joga o valor para a variavel o primeiro elemento de acordo com o id 
            var cardapio = cardapios.FirstOrDefault(c => c.Id == id);
            if (cardapio is null)
            {
                //se nao encontrar o id retorna not found
                return Results.NotFound("Cárdapio não encontrado!");
            }


            //retorna o valor para o endpoint da api
            return Results.Ok(cardapio);
        }

        // POST api/<CardapioItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CardapioItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CardapioItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
