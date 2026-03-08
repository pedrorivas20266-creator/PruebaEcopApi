namespace api_prueba_ecop.src.Application.Models.Requests;
public class PedidoDetalleRequest
{
    public int CodProducto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public int LineaNumero { get; set; }
}