using Comandas.Api.DTOs;
using Comandas.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Comandas.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComandaController : ControllerBase
{
    private readonly ComandasDbContext _context;

    public ComandaController(ComandasDbContext context)
    {
        _context = context;
    }
    // GET: api/<ComandaController>
    [HttpGet]
    public IResult Get()
    {
        var comandas = _context.Comandas
            .Select(c => new ComandaCreateResponse
            {
                NomeCliente = c.NomeCliente,
                NumeroMesa = c.NumeroMesa,
                Itens = c.Itens.Select(i => new ComandaItemResponse
                {
                    Id = i.Id,
                    Titulo = _context.CardapioItens
                        .First(ci => ci.Id == i.CardapioItemId).Titulo
                }).ToList()
            })
            .ToList();

        return Results.Ok(comandas);
    }

    // GET api/<ComandaController>/5
    [HttpGet("{id}")]
    public IResult Get(int id)
    {
        var comanda = _context.Comandas.
            Select(c => new ComandaCreateResponse
            {
                Id = id,
                NomeCliente = c.NomeCliente,
                NumeroMesa = c.NumeroMesa,
                Itens = c.Itens.Select(i => new ComandaItemResponse
                {
                    Id = i.Id,
                    Titulo = _context.CardapioItens
                        .First(ci => ci.Id == i.CardapioItemId).Titulo
                }).ToList()
            })
                .FirstOrDefault(c => c.Id == id);
        if (comanda is null)
            return Results.NotFound("Comanda nao encontrada");
        return Results.Ok(comanda);
    }

    // POST api/<ComandaController>
    [HttpPost]
    public IResult Post([FromBody] ComandaCreateRequest comandaCreate)
    {
        if (comandaCreate.NomeCliente.Length < 3)
            return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
        if (comandaCreate.NumeroMesa <= 0)
            return Results.BadRequest("O número da mesa deve ser maior que zero.");
        if (comandaCreate.CardapioItemIds.Length == 0)
            return Results.BadRequest("A comanda deve ter pelo menos um item do cardápio.");
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
                Comanda = novaComanda
            };

            comandaItens.Add(comandaItem);

            var cardapioItem = _context.CardapioItens
                .FirstOrDefault(ci => ci.Id == cardapioItemId);

            if (cardapioItem!.PossuiPreparo)
            {
                var pedido = new PedidoCozinha
                {
                    Comanda = novaComanda
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
        // cria a resposta da requisição
        var resposta = new ComandaCreateResponse
        {
            Id = novaComanda.Id,
            NomeCliente = novaComanda.NomeCliente,
            NumeroMesa = novaComanda.NumeroMesa,
            Itens = novaComanda.Itens.Select(i => new ComandaItemResponse
            {
                Id = i.Id,
                Titulo = _context.CardapioItens
                    .First(ci => ci.Id == i.CardapioItemId).Titulo
            }).ToList()
        };
        return Results.Created($"/api/comanda/{resposta.Id}", resposta);
    }

    // PUT api/<ComandaController>/5
    [HttpPut("{id}")]
    public IResult Put(int id, [FromBody] ComandaUpdateRequest comandaUpdate)
    {
        var comanda = _context.Comandas.FirstOrDefault(c => c.Id == id);
        if (comanda is null) // se não encontrou a comanda pesquisda
            return Results.NotFound("Comanda nao encontrada");
        if (comandaUpdate.NomeCliente.Length < 3)
            return Results.BadRequest("O nome do cliente deve ter no mínimo 3 caracteres.");
        if (comandaUpdate.NumeroMesa <= 0)
            return Results.BadRequest("O número da mesa deve ser maior que zero.");

        comanda.NomeCliente = comandaUpdate.NomeCliente;
        comanda.NumeroMesa = comandaUpdate.NumeroMesa;

        foreach (var item in comandaUpdate.Itens)
        {
            if (item.Id > 0 && item.Remove == true)
            {
                RemoverItemComanda(item.Id);
            }

            if (item.CardapioItemId > 0)
            {
                InserirItemComanda(comanda, item.CardapioItemId);
            }
        }

        _context.SaveChanges();

        return Results.NoContent();
    }

    private void InserirItemComanda(Comanda comanda, int cardapioItemId)
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
        // consulta o item da comanda pelo id
        var comandaItem = _context.ComandaItens.FirstOrDefault(ci => ci.Id == id);
        if (comandaItem is not null)
        {
            // remove o item da comanda
            _context.ComandaItens.Remove(comandaItem);
        }
    }





    // DELETE api/<ComandaController>/5
    [HttpDelete("{id}")]
    public IResult Delete(int id)
    {
        // pesquisa uma comanda na lista de comandas pelo id da comanda
        // que veio no parametro da request
        var comanda = _context.Comandas
                .FirstOrDefault(c => c.Id == id);
        // se não encontrou a comanda pesquisda
        if (comanda is null)
            // retorna um codigo 404 Não encontrado
            return Results.NotFound("Comanda nao encontrada");
        // remove a comanda da lista de comandas
        _context.Comandas.Remove(comanda);
        var removidoComSucesso = _context.SaveChanges();
        if (removidoComSucesso > 0)
            // retorna 204 Sem conteudo
            return Results.NoContent();
        return Results.StatusCode(500);
    }
}