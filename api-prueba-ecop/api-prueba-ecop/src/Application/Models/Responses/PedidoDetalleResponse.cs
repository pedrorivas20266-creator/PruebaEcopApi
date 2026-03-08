namespace api_prueba_ecop.src.Application.Models.Responses;
public class PedidoDetalleResponse
{
    public int CodPedidoDetalle { get; set; }
    public int CodProducto { get; set; }
    public string? NombreProducto { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public decimal? Subtotal { get; set; }
    public int LineaNumero { get; set; }
}