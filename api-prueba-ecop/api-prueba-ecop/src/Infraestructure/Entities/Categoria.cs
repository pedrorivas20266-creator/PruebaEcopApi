namespace api_prueba_ecop.src.Infraestructure.Entities;
public class Categoria
{
    public int CodCategoria { get; set; }
    public string? NumCategoria { get; set; }
    public string? DesCategoria { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FecGra { get; set; }
}