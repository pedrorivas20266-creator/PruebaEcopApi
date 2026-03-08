namespace api_prueba_ecop.src.Application.Models.Requests;
public class PedidoRequest
{
    public string? NumPedido { get; set; }
    public DateTime? Fecha { get; set; }
    public int CodUsuario { get; set; }
    public int CodCliente { get; set; }
    public int CodMoneda { get; set; }
    public required List<PedidoDetalleRequest> Detalles { get; set; }
}