using api_prueba_ecop.src.Application.Interfaces;
using api_prueba_ecop.src.Infraestructure.Data;
using api_prueba_ecop.src.Infraestructure.Entities;
using api_prueba_ecop.src.Infraestructure.Predicate;
using Microsoft.EntityFrameworkCore;

namespace api_prueba_ecop.src.Infraestructure.Repository;
public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> ObtenerCliente(int codigoCliente, CancellationToken token)
    {
        var cliente = await _context
            .Cliente
            .AsNoTracking()
            .Where(x => x.CodCliente == codigoCliente)
            .FirstOrDefaultAsync(token);

        return cliente;
    }

    public async Task<List<Cliente>?> ObtenerClientesActivos(CancellationToken token)
    {
        var clientes = await _context
            .Cliente
            .AsNoTracking()
            .Where(x => x.Activo)
            .ToListAsync(token);

        return clientes;
    }

    public async Task<List<Cliente>?> ObtenerClientesPorFiltro(string? busqueda, int limite, CancellationToken token)
    {
        var filtro = ClientePredicate.Build(busqueda);

        var query = _context
            .Cliente
            .AsNoTracking()
            .Where(filtro)
            .OrderBy(x => x.CodCliente)
            .AsQueryable();

        query = query.Take(limite);

        var clientes = await query.ToListAsync(token);
        return clientes;
    }

    public async Task<Cliente> InsertarClienteNuevo(Cliente cliente, CancellationToken token)
    {
        var clienteCreado = await _context.Cliente.AddAsync(cliente, token);
        await _context.SaveChangesAsync(token);
        return clienteCreado.Entity;
    }

    public async Task<Cliente?> ActualizarCliente(int id, Cliente clienteActualizado, CancellationToken token)
    {
        var cliente = await _context.Cliente.FindAsync(new object[] { id }, token);

        if (cliente == null)
            return null;

        cliente.NumCliente = clienteActualizado.NumCliente;
        cliente.Nombres = clienteActualizado.Nombres;
        cliente.Apellidos = clienteActualizado.Apellidos;
        cliente.CodTipoDocumento = clienteActualizado.CodTipoDocumento;
        cliente.NumeroDocumento = clienteActualizado.NumeroDocumento;
        cliente.NumeroTelefono = clienteActualizado.NumeroTelefono;
        cliente.Correo = clienteActualizado.Correo;
        cliente.Direccion = clienteActualizado.Direccion;
        cliente.Activo = clienteActualizado.Activo;

        await _context.SaveChangesAsync(token);

        return cliente;
    }

    public async Task<bool> EliminarCliente(int id, CancellationToken token)
    {
        var cliente = await _context.Cliente.FindAsync(new object[] { id }, token);

        if (cliente == null)
            return false;

        _context.Cliente.Remove(cliente);

        await _context.SaveChangesAsync(token);

        return true;
    }
}
