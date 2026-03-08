using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Models.Responses;
public class ProductoConPrecioResponse
{
    public int CodProducto { get; set; }
    public string? NumProducto { get; set; }
    public string? CodigoBarra { get; set; }
    public string? DesProducto { get; set; }
    public int CodCategoria { get; set; }
    public int CodUnidadMedida { get; set; }
    public int CodIva { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public decimal? CostoPromedio { get; set; }
    public decimal? CostoUltimo { get; set; }
    public bool Activo { get; set; }
    public bool DescuentaStock { get; set; }
    public DateTime? FecGra { get; set; }
    public List<PrecioResponse>? Precios { get; set; }
}

public class PrecioResponse
{
    public int CodPrecio { get; set; }
    public string? NumPrecio { get; set; }
    public string? DesPrecio { get; set; }
    public int CodTipoPrecio { get; set; }
    public string? DesTipoPrecio { get; set; }
    public decimal? PrecioVenta { get; set; }
    public bool Activo { get; set; }
}