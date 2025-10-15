namespace Comandas.Api.DTOs
{
    public class ComandaCreateRequest
    {
        public int ComandaItemId { get; set; }
        public int[] CardapioItemIds { get; set; } = default!;
        public int NumeroMesa { get; internal set; }
        public string NomeCliente { get; internal set; }
    }
}
