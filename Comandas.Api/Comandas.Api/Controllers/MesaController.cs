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
        private static List<Comanda> mesas = new List<Comanda>();

        // GET: api/<MesaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MesaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MesaController>
        [HttpPost]
        public IResult Post([FromBody] MesaCreateRequest mesaCreate)
        {
            if (mesaCreate.NumeroMesa.ToString().Length < 3) // Convert 'NumeroMesa' to string to check its length
                return Results.BadRequest("O número da mesa deve ter no mínimo 3 caracteres.");
            if (mesaCreate.SituacaoMesa < 0 || mesaCreate.SituacaoMesa > 2)
                return Results.BadRequest("A situação da mesa deve ser 0 (livre), 1 (ocupada) ou 2 (reservada).");

            var novaMesa = new Comanda
            {
                Id = mesas.Count + 1,
                NomeCliente = mesaCreate.NomeCliente,
                NumeroMesa = mesaCreate.NumeroMesa,
                SituacaoMesa = mesaCreate.SituacaoMesa
            };

            mesas.Add(novaMesa);
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

            var mesa = mesas.FirstOrDefault(m => m.Id == id);
            if (mesa is null)
                return Results.NotFound($"Mesa do id {id} não encontrada");

            // Atualiza os dados da mesa
            mesa.NumeroMesa = mesaUpdate.NumeroMesa;
            mesa.SituacaoMesa = mesaUpdate.SituacaoMesa;

            // Retorna sem conteúdo
            return Results.NoContent();
        }

        // DELETE api/<MesaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            var mesa = mesas.FirstOrDefault(m => m.Id == id);
            if (mesa is null)
                return Results.NotFound($"Mesa do id {id} não encontrada");

            mesas.Remove(mesa);
            return Results.NoContent();
        }
    }
}
