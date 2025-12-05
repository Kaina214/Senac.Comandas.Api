namespace Comandas.Api.Controllers
{
    internal class PedidoCozinhaResponse
    {
        public int Id { get; set; }
        public int ComandaId { get; set; }
        public object Itens { get; set; }
    }
}