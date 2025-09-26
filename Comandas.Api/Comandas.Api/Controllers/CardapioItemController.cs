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
        //METODO GET que retorna a lista de cardápio
        // GET: api/<CardapioItemController>
        [HttpGet]
        public IEnumerable<CardapioItem> Get()
        {
            return new CardapioItem[] 
            {
                new CardapioItem//Criar o primeiro elemento da lista de cardapio
                {
                    Id = 1,
                    Titulo = "Coxinha",
                    Descricao = "Deliciosa coxinha de frango com catupiry",
                    Preco = 5.50m,
                    PossuiPreparo = true
                },
                new CardapioItem//Criar o segundo elemento da lista de cardapio
                {
                    Id = 2,
                    Titulo = "X-Salada",
                    Descricao = "Bife, Alface, cebola, tomate,maionese, mostarda, tempero verde",
                    Preco = 25.50m,
                    PossuiPreparo = true

                }

            };
        }

        // GET api/<CardapioItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
