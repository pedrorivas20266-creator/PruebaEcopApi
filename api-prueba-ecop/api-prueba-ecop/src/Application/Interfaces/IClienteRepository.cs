using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IClienteRepository
{
    Task<Cliente?> ObtenerCliente(int codigoCliente, CancellationToken token);
    Task<List<Cliente>?> ObtenerClientesActivos(CancellationToken token);
    Task<Cliente> InsertarClienteNuevo(Cliente cliente, CancellationToken token);
    Task<Cliente?> ActualizarCliente(int id, Cliente clienteActualizado, CancellationToken token);
    Task<bool> EliminarCliente(int id, CancellationToken token);
    Task<List<Cliente>?> ObtenerClientesPorFiltro(string? busqueda, int limite, CancellationToken token);
}
