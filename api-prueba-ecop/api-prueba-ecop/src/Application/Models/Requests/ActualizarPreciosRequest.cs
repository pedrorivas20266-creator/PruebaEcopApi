namespace api_prueba_ecop.src.Application.Models.Requests;

public class ActualizarPreciosRequest
{
    public List<ActualizarPrecioItem> Precios { get; set; } = new();
}

public class ActualizarPrecioItem
{
    public int CodPrecio { get; set; }
    public decimal PrecioVenta { get; set; }
}