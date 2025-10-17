using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoCozinhaController : ControllerBase
    {
       static List<PedidoCozinha> pedidos = new List<PedidoCozinha>()
        {
           new PedidoCozinha
           {
               Id = 1,
               ComandaItemId = 1,
           },
              new PedidoCozinha
              {
                Id = 2,
                ComandaItemId = 2,
               }
        };

        // GET: api/<PedidoCozinhaController>
        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(pedidos);
        }

        // GET api/<PedidoCozinhaController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var PedidoCozinha = pedidos.FirstOrDefault(p => p.Id == id);
            if (PedidoCozinha is null)
            {
                return Results.NotFound("Pedido não encontrado!");
            }
            return Results.Ok(PedidoCozinha);
        }

        // POST api/<PedidoCozinhaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PedidoCozinhaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PedidoCozinhaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var PedidoCozinha = pedidos.
                FirstOrDefault(p => p.Id == id);

            if (PedidoCozinha is null)
                return Results.NotFound($"Pedido {id} não encontrado!");

            var  removido = pedidos.Remove(PedidoCozinha);
            if(removido)
                return Results.NoContent();
            return Results.StatusCode(500);


        }
    }
}
