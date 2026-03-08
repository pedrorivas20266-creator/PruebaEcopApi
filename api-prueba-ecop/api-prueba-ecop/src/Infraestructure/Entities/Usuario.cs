namespace api_prueba_ecop.src.Infraestructure.Entities;
public class Usuario
{
    public int CodUsuario { get; set; }
    public string NumUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string AccesoUsuario { get; set; } = string.Empty;
    public required byte[] AccesoClave { get; set; }
    public bool Activo { get; set; }
}
