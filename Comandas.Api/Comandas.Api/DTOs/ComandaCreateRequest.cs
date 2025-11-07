namespace Comandas.Api.DTOs
{
    public class ComandaCreateRequest
    {
        public int ComandaItemId { get; set; }
        public int[] CardapioItemIds { get; set; } = default!;
        public int NumeroMesa { get;  set; }
        public string NomeCliente { get;  set; }
    }
}
