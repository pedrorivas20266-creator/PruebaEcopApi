namespace api_prueba_ecop.src.Infraestructure.Entities;
public class TipoPrecio
{
    public int CodTipoPrecio { get; set; }
    public string? NumTipoPrecio { get; set; }
    public string? DesTipoPrecio { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FecGra { get; set; }
}