namespace Comandas.Api.DTOs
{
    public class MesaCreateRequest
    {
        public int NumeroMesa { get; set; }
        public int SituacaoMesa { get; set; }
        public string? NomeCliente { get; internal set; }
    }
}
