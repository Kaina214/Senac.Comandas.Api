using System.Reflection.Metadata.Ecma335;

namespace Comandas.Api.Models
{
    public class PedidoCozinha
    {
        public int Id { get; set; }
        public int ComandaItemId { get; set; }
        public List<PedidoCozinhaItem> Itens { get; set; } = [];
        public int Comanda { get; internal set; }
    }
}
