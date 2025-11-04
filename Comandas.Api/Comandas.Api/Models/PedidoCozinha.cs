using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Comandas.Api.Models
{
    public class PedidoCozinha
    {
        [Key]//pk
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ComandaId { get; set; }//fk
        public virtual Comanda Comanda { get; set; }
        public List<PedidoCozinhaItem> Itens { get; set; } = [];
    
    }
}
