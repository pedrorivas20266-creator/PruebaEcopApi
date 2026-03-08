using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IUsuarioRepository
{
    Task<Usuario?> ValidarLogin(string nombreUsuario, byte[] claveUsuario, CancellationToken token);
}
