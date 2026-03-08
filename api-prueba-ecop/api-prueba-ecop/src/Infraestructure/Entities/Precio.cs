namespace api_prueba_ecop.src.Infraestructure.Entities;
public class Precio
{
    public int CodPrecio { get; set; }
    public int CodProducto { get; set; }
    public int CodTipoPrecio { get; set; }
    public decimal? PrecioVenta { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FecGra { get; set; }
    public TipoPrecio? TipoPrecio { get; set; }
}