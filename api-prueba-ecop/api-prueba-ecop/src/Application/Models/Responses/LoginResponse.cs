namespace api_prueba_ecop.src.Application.Models.Responses;
public class LoginResponse
{
    public required UsuarioResponse Usuario { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public TokenType TokenType { get; set; }
    public int ExpiresIn { get; set; }
}

public class UsuarioResponse
{
    public int CodUsuario { get; set; }
    public string NumUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
}

public enum TokenType
{
    Bearer
}
