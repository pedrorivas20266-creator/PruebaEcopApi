namespace api_prueba_ecop.src.Infraestructure.Entities;
public class PedidoDetalle
{
    public int CodPedidoDetalle { get; set; }
    public int CodPedido { get; set; }
    public int CodProducto { get; set; }
    public decimal? Cantidad { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public int LineaNumero { get; set; }
    public DateTime? FecGra { get; set; }

    public Producto? Producto { get; set; }
}