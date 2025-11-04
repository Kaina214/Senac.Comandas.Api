using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comandas.Api.Models
{
    public class PedidoCozinhaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PedidoCozinhaId { get; set; }//fk
        public PedidoCozinha PedidoCozinha { get; set; }//Navegação
        public int ComandaItemId { get; set; }//fk
        public ComandaItem ComandaItem { get; set; }//Navegação
    }
}
