using api_prueba_ecop.src.Infraestructure.Entities;

namespace api_prueba_ecop.src.Application.Interfaces;
public interface IClienteService
{
    Task<List<Cliente>> ObtenerClientesActivos(CancellationToken token);
    Task<List<Cliente>> ObtenerClientesPorFiltro(string? busqueda, int? limite, CancellationToken token);
    Task<Cliente> ObtenerCliente(int codigoCliente, CancellationToken token);
    Task<Cliente> NuevoCliente(Cliente cliente, CancellationToken token);
    Task<Cliente> ActualizarCliente(int codigoCliente, Cliente cliente, CancellationToken token);
    Task EliminarCliente(int codigoCliente, CancellationToken token);
}
