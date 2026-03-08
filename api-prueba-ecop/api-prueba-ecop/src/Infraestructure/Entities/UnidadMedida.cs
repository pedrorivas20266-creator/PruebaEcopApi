namespace api_prueba_ecop.src.Infraestructure.Entities;
public class UnidadMedida
{
    public int CodUnidadMedida { get; set; }
    public string? NumUnidadMedida { get; set; }
    public string? DesUnidadMedida { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FecGra { get; set; }
}