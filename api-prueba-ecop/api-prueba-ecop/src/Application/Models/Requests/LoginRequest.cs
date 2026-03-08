namespace api_prueba_ecop.src.Application.Models.Requests;
public class LoginRequest
{
    public required string NombreUsuario { get; set; }
    public required string ClaveUsuario { get; set; }
}
