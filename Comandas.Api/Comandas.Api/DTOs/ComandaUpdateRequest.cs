namespace Comandas.Api.DTOs
{
    public class ComandaUpdateRequest
    {
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; } = default!;
        public ComandaItemUpdateRequest[] Itens { get; set; } = [];//lista
    }
    public class ComandaItemUpdateRequest
    {
        public int Id { get; set; }//id da comanda item
        public bool Remove { get; set; }//indicar se esta removendo
        public int CardapioItemId { get; set; }//indicar se estar inserindo
    }
}
