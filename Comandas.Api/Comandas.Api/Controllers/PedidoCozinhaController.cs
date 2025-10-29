using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoCozinhaController : ControllerBase
    {
       public ComandasDbContext _context { get; set; }

        public PedidoCozinhaController(ComandasDbContext context)
        {
            _context = context;
        }

        // GET: api/<PedidoCozinhaController>
        [HttpGet]
        public IResult Get()
        {
           var pedidos = _context.PedidoCozinhas.ToList();
            return Results.Ok(pedidos);
        }

        // GET api/<PedidoCozinhaController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var PedidoCozinha = _context.PedidoCozinhas.
                FirstOrDefault(p => p.Id == id);
            if (PedidoCozinha is null)
            {
                return Results.NotFound("Pedido não encontrado!");
            }
            return Results.Ok(PedidoCozinha);
        }

        // POST api/<PedidoCozinhaController>
        [HttpPost]
        public IResult Post([FromBody] PedidoCozinhaCreateRequest pedido)
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
            var PedidoCozinha = _context.PedidoCozinhas.
                FirstOrDefault(p => p.Id == id);

            if (PedidoCozinha is null)
                return Results.NotFound($"Pedido {id} não encontrado!");
            _context.PedidoCozinhas.Remove(PedidoCozinha);
            var removido = _context.SaveChanges();
            if (removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);


        }
    }
}
