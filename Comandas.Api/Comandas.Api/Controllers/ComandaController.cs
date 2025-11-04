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
        public ComandasDbContext _context { get; set; }

        public ComandaController(ComandasDbContext context)
        {
            _context = context;
        }



        // GET: api/<ComandaController>
        [HttpGet]
        public IResult Get()
        {
            var comandas = _context.Comandas.ToList();
            return Results.Ok(comandas);
        }

        // GET api/<ComandaController>/5
        [HttpGet("{id}")]
        public IResult Get(int id)
        {
            var comanda = _context.Comandas.
                FirstOrDefault(c => c.Id == id);
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
                
                NomeCliente = comandaCreate.NomeCliente,
                NumeroMesa = comandaCreate.NumeroMesa,
            };
             var comandaItens = new List<ComandaItem>();
            foreach (int cardapioItemId in comandaCreate.CardapioItemIds)
            {
                var comandaItem = new ComandaItem
                {

                    CardapioItemId = cardapioItemId,
                    Comanda = novaComanda,
                };
                // adiciona o item à lista de itens 
                comandaItens.Add(comandaItem);
                //Criar o pedido de coainha e o item de acordo com o cadastro do cardapio possuipreparo
                var cardapioItem = _context.CardapioItens.
                     FirstOrDefault(ci => ci.Id == cardapioItemId);
                if (cardapioItem!.PossuiPreparo)
                {
                    var pedido = new PedidoCozinha
                    {
                        Comanda = novaComanda,

                    };
                    var pedidoItem = new PedidoCozinhaItem
                    {
                        ComandaItem = comandaItem,
                        PedidoCozinha = pedido
                    };
                    _context.PedidoCozinhas.Add(pedido);
                    _context.PedidoCozinhaItens.Add(pedidoItem);
                }
            
                novaComanda.Itens = comandaItens;
                _context.Comandas.Add(novaComanda);
                _context.SaveChanges();
                return Results.Created($"/api/comanda/{novaComanda.Id}", novaComanda);


            }

          
            return Results.Created($"/api/comanda/{novaComanda.Id}", novaComanda);
        }


        // PUT api/<ComandaController>/5
        [HttpPut("{id}")]
        public IResult Put(int id, [FromBody] ComandaUpdateRequest comandaUpdate)
        {
            var comanda = _context.Comandas.
                FirstOrDefault(c => c.Id == id);
            
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

            _context.SaveChanges();
            return Results.NoContent();

        }

        // DELETE api/<ComandaController>/5
        [HttpDelete("{id}")]
        public IResult Delete(int id)
        {
            //pesquisa uma comanda na lista de comandas pelo id da comanda
            //que veio no parametro da request
            var comanda = _context.Comandas.
                FirstOrDefault(c => c.Id == id);
            // se não encontra a comanda pesquisada
            if (comanda is null)
                //retorna um código 404 not found não encontrado
            return Results.NotFound("Comanda não encontrada!");
            //remove a comanda da lista de comandas
            _context.Comandas.Remove(comanda);
            var removidoComSucesso = _context.SaveChanges();

            if (removidoComSucesso > 0)
            {
                // retorna 204 sem conteudo
                return Results.NoContent();
            }
            return Results.StatusCode(500);
        }
    }
}
