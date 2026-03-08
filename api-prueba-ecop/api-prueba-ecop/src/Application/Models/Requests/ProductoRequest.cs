namespace api_prueba_ecop.src.Application.Models.Requests;

public class ProductoRequest
{
    public string? NumProducto { get; set; }
    public string? CodigoBarra { get; set; }
    public string? DesProducto { get; set; }
    public int CodCategoria { get; set; }
    public int CodUnidadMedida { get; set; }
    public int CodIva { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public decimal? CostoPromedio { get; set; }
    public decimal? CostoUltimo { get; set; }
    public bool DescuentaStock { get; set; } = true;
    public List<PrecioRequest>? Precios { get; set; }
}

public class PrecioRequest
{
    public string? NumPrecio { get; set; }
    public string? DesPrecio { get; set; }
    public int CodTipoPrecio { get; set; }
    public decimal? PrecioVenta { get; set; }
}