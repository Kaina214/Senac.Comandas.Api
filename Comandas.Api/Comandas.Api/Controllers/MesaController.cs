using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
         public ComandasDbContext _context { get; set; }

        public MesaController (ComandasDbContext context)
        {
            _context = context;
        }


        // GET: api/<MesaController>
        [HttpGet]
        public IResult Get()
        {
           var mesas = _context.Mesas.ToList();
            return Results.Ok(mesas);
        }

        // GET api/<MesaController>/5
        [HttpGet("{id}")]
        public  IResult Get(int id)
        {
           var mesa = _context.Mesas.
                FirstOrDefault(m => m.Id == id);
            if (mesa is null)
            {
                return Results.NotFound("Mesa não encontrada!");
            }
            return Results.Ok(mesa);
        }

        // The issue arises because the code is trying to add an object of type `Comanda` to the `Mesas` DbSet, which expects objects of type `Mesa`.
        // To fix this, the object `novaMesa` should be of type `Mesa` instead of `Comanda`.

        [HttpPost]
        public IResult Post([FromBody] MesaCreateRequest mesaCreate)
        {
            if (mesaCreate.NumeroMesa.ToString().Length < 3) // Convert 'NumeroMesa' to string to check its length
                return Results.BadRequest("O número da mesa deve ter no mínimo 3 caracteres.");
            if (mesaCreate.SituacaoMesa < 0 || mesaCreate.SituacaoMesa > 2)
                return Results.BadRequest("A situação da mesa deve ser 0 (livre), 1 (ocupada) ou 2 (reservada).");

            // Create a new Mesa object instead of Comanda
            var novaMesa = new Mesa
            {
                NumeroMesa = mesaCreate.NumeroMesa,
                SituacaoMesa = mesaCreate.SituacaoMesa
            };

            _context.Mesas.Add(novaMesa); // Add the Mesa object to the Mesas DbSet
            _context.SaveChanges();
            return Results.Created($"/api/mesa/{novaMesa.Id}", novaMesa);
        }

        // PUT api/<MesaController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] MesaUpdateRequest mesaUpdate)
        {
            if (mesaUpdate.NumeroMesa <= 0)
                return Results.BadRequest("O número da mesa deve ser maior que zero.");
            if (mesaUpdate.SituacaoMesa < 0 || mesaUpdate.SituacaoMesa > 2)
                return Results.BadRequest("A situação da mesa deve ser 0 (livre), 1 (ocupada) ou 2 (reservada).");

            var mesa = _context.Mesas.
                FirstOrDefault(m => m.Id == id);
            if (mesa is null)
                return Results.NotFound($"Mesa do id {id} não encontrada");

            // Atualiza os dados da mesa
            mesa.NumeroMesa = mesaUpdate.NumeroMesa;
            mesa.SituacaoMesa = mesaUpdate.SituacaoMesa;

            _context.SaveChanges();
            // Retorna sem conteúdo
            return Results.NoContent();
        }

        // DELETE api/<MesaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var mesa = _context.Mesas.
                FirstOrDefault(m => m.Id == id);
            if (mesa is null)
                return Results.NotFound($"Mesa do id {id} não encontrada");
            _context.Mesas.Remove(mesa);
            var removido = _context.SaveChanges();
            if(removido > 0)
            {
                return Results.NoContent();
            }
            return Results.StatusCode(500);
           
           
        }
    }
}
