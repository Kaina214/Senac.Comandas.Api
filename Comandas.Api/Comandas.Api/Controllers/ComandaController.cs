using Azure;
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
            return Results.Ok(comanda);
        }

        // POST api/<ComandaController>
        [HttpPost]
        public IResult Post([FromBody] ComandaCreateRequest comandaCreate)
        {
            if (comandaCreate.NomeCliente.Length < 3)
                return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
            if (comandaCreate.NumeroMesa <= 0)
                return Results.BadRequest("O número da mesa nao pode ser zero");
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

            }
            novaComanda.Itens = comandaItens;
            _context.Comandas.Add(novaComanda);
            _context.SaveChanges();
            var response = new ComandaCreateResponse
            {
                Id = novaComanda.Id,
                NomeCliente = novaComanda.NomeCliente,
                NumeroMesa = novaComanda.NumeroMesa,
                Itens = novaComanda.Itens.Select(i => new ComandaItemResponse
                {
                    Id = i.Id,
                    Titulo = _context.CardapioItens.First(ci => ci.Id == i.CardapioItemId).Titulo

                }).ToList()


            };
            return Results.Created($"/api/comanda/{response.Id}", response);
        }
        // PUT api/<ComandaController>/5
            [HttpPut("{id}")]
            public IResult Put(int id, [FromBody] ComandaUpdateRequest comandaUpdate)
            {
                //pesquisa uma comanda na lista de comandas pelo id da comanda que veio no parametro de registro
                var comanda = _context.Comandas.FirstOrDefault(u => u.Id == id);
                if (comanda is null) // se não encontrou a comanda pesquisada
                    return Results.NotFound("Comanda nao encontrada");
                // valida os dados da comanda
                if (comandaUpdate.NomeCliente.Length < 3)
                    return Results.BadRequest("O nome deve ter no minimo 3 caracteres");
                // valida os numero da mesa
                if (comandaUpdate.NumeroMesa <= 0)
                    return Results.BadRequest("O numero da mesa deve ser maior que zero");
                // atualiza as infomações da comanda
                comanda.NomeCliente = comandaUpdate.NomeCliente;
                comanda.NumeroMesa = comandaUpdate.NumeroMesa;

            foreach (var item in comandaUpdate.Itens)
            {
                //se Id for informado e o rende for verdadeiro
                if (item.Id > 0 && item.Remove == true)
                {
                    RemoverItemComanda(item.Id);

                    //removendo
                }    
                //se o cardaitemid for informado 
                if(item.CardapioItemId >0)
                {
                    IserirItemComanda(comanda, item.CardapioItemId);

                    //inserindo
                }

            }
            _context.SaveChanges();
            // retorna 204 sem conteudo
            return Results.NoContent();
            }

        private void IserirItemComanda(Comanda comanda, int cardapioItemId)
        {
            _context.ComandaItens.Add(
                new ComandaItem
            {
                
                CardapioItemId = cardapioItemId,
                Comanda = comanda
                }
                );
        }

        private void RemoverItemComanda(int id)
        {
            var comandaItem = _context.ComandaItens.FirstOrDefault(ci => ci.Id == id);
            if (comandaItem is not null)
            {
                _context.ComandaItens.Remove(comandaItem);
            }
        }
       

        // DELETE api/<ComandaController>/5
        [HttpDelete("{id}")]
            public IResult Delete(int id)
            {
                var comanda = _context.Comandas.FirstOrDefault(u => u.Id == id);
                if (comanda is null)
                    return Results.NotFound("Comanda nao encontrada");

                var removidoComSucessoComanda = _context.Comandas.Remove(comanda);

                _context.Comandas.Remove(comanda);
                var removido = _context.SaveChanges();
                if (removido > 0)
                {
                    return Results.NoContent();
                }
                return Results.StatusCode(500);
            }
        
    }
}
