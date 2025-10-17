using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Comandas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComandaController : ControllerBase
    {
       static List<Comanda> comandas = new List<Comanda>()
        {
           new Comanda
           {
               Id = 1,
               NomeCliente = "Kainã Habekost",
               NumeroMesa = 1,
               Itens = new List<ComandaItem>
               {
                 new ComandaItem
                  {

                   Id = 1,
                   CardapioItemId = 1,
                   ComandaId = 1,

                   }

           },   },
           new Comanda
           {
               Id = 2,
               NomeCliente = "Ana Clara",
               NumeroMesa = 2,
                Itens = new List<ComandaItem>
                {
                  new ComandaItem
                    {

                     Id = 2,
                     CardapioItemId = 2,
                     ComandaId = 1,

                     }
                  },
            }
        };

       

        // GET: api/<ComandaController>
        [HttpGet]
        public IResult Get()
        {
            return Results.Ok(comandas);
        }

        // GET api/<ComandaController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var comanda = comandas.FirstOrDefault(c => c.Id == id);
            if (comanda is null)
            {
                return Results.NotFound("Comanda não encontrada!");
            }
            return Results.Ok(comanda) ;
        }

        // POST api/<ComandaController>
        [HttpPost]
        public IResult Post([FromBody] ComandaCreateRequest comandaCreate)
        {
            if (comandaCreate.NomeCliente.Length < 3)
                return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
            if (comandaCreate.NumeroMesa <= 3)
                return Results.BadRequest("O número da mesa deve ter no mínimo 3 caracteres.");
            var novaComanda = new Comanda
            {
                Id = comandas.Count + 1,
                NomeCliente = comandaCreate.NomeCliente,
                NumeroMesa = comandaCreate.NumeroMesa,
            };
            foreach (var item in comandaCreate.CardapioItemIds)
            {
                var novoItem = new ComandaItem
                {
                    Id = novaComanda.Itens.Count + 1,
                    CardapioItemId = item,
                    ComandaId = novaComanda.Id,
                };
                novaComanda.Itens.Add(novoItem);
            }

            comandas.Add(novaComanda);
            return Results.Created($"/api/comanda/{novaComanda.Id}", novaComanda);
        }


        // PUT api/<ComandaController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] ComandaUpdateRequest comandaUpdate)
        {
            var comanda = comandas.FirstOrDefault(c => c.Id == id);
            
            if (comanda is null)

                return Results.NotFound("Comanda não encontrada!");
            if (comandaUpdate.NomeCliente.Length < 3)
                return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
            //valida o numero da mesa
            if (comandaUpdate.NumeroMesa <= 0)
                return Results.BadRequest("O número da mesa deve ser maior que zero.");
            //atualiza os dados da comanda

            comanda.NomeCliente = comandaUpdate.NomeCliente;
            comanda.NumeroMesa = comandaUpdate.NumeroMesa;
            //retorna 204 sem conteudo 
            return Results.NoContent();

        }

        // DELETE api/<ComandaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            //pesquisa uma comanda na lista de comandas pelo id da comanda
            //que veio no parametro da request
            var comanda = comandas.FirstOrDefault(c => c.Id == id);
            // se não encontra a comanda pesquisada
            if (comanda is null)
                //retorna um código 404 not found não encontrado
            return Results.NotFound("Comanda não encontrada!");
            //remove a comanda da lista de comandas
            var removidoComSucesso = comandas.Remove(comanda);
          
            if(removidoComSucesso)
                // retorna 204 sem conteudo
                return Results.NoContent();
            return Results.StatusCode(500);
        }
    }
}
