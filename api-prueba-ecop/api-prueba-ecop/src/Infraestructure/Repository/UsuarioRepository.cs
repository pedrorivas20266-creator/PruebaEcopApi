using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api_prueba_ecop.src.Infraestructure.Repository;
public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuarioRepository> _logger;

    public UsuarioRepository(AppDbContext context, ILogger<UsuarioRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Usuario?> ValidarLogin(string nombreUsuario, byte[] claveUsuario, CancellationToken token)
    {
        var usuario = await _context
            .Usuario
            .AsNoTracking()
            .Where(x => x.AccesoUsuario == nombreUsuario && x.Activo == true)
            .FirstOrDefaultAsync(token);

        if (usuario != null)
        {
            if (!usuario.AccesoClave.SequenceEqual(claveUsuario))
            {
                _logger.LogWarning("Los hashes no coinciden");
                return null;
            }
        }

        return usuario;
    }
}
