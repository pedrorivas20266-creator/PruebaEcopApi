namespace api_prueba_ecop.src.Infraestructure.Entities;

public class Iva
{
    public int CodIva { get; set; }
    public string? NumIva { get; set; }
    public string? DesIva { get; set; }
    public decimal Coeficiente { get; set; }
    public int Divisor { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FecGra { get; set; }
}
