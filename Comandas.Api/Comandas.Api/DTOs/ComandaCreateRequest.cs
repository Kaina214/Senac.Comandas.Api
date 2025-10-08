namespace Comandas.Api.DTOs
{
    public class ComandaCreateRequest
    {
        public int ComandaItemId { get; set; }
        public int CardapioItemId { get; set; } = default;
        public int[] CardapioItemIds { get; set; } = default!;
    }
}
