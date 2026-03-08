namespace api_prueba_ecop.src.Infraestructure.Entities;
public class Pedido
{
    public int CodPedido { get; set; }
    public string? NumPedido { get; set; }
    public DateTime? Fecha { get; set; }
    public int CodUsuario { get; set; }
    public int CodCliente { get; set; }
    public int CodMoneda { get; set; }
    public decimal? Total { get; set; }
    public decimal? Iva { get; set; }
    public bool Activo { get; set; } = true;
    public string? MotivoAnulacion { get; set; }
    public DateTime? FecGra { get; set; }
    public Cliente? Cliente { get; set; }
    public required List<PedidoDetalle> Detalles { get; set; }
}