using api_prueba_ecop.src.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_prueba_ecop.src.Infraestructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    #region"Esquemas"
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<UnidadMedida> UnidadMedida { get; set; }
    public DbSet<TipoPrecio> TipoPrecio { get; set; }
    public DbSet<Precio> Precio { get; set; }
    public DbSet<Pedido> Pedido { get; set; }
    public DbSet<PedidoDetalle> PedidoDetalle { get; set; }
    public DbSet<Iva> Iva { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder model)
    {
        #region"Usuario"
        model.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.CodUsuario);
            entity.Property(e => e.AccesoClave).HasColumnName("accesopass").HasColumnType("varbinary(255)");
            entity.Property(e => e.Activo).HasColumnName("activo").HasColumnType("bit");
        });
        #endregion

        #region"Cliente"
        model.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CodCliente);

            entity.Property(e => e.NumCliente)
                .HasColumnName("numcliente")
                .HasMaxLength(55);

            entity.Property(e => e.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(255);

            entity.Property(e => e.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(255);

            entity.Property(e => e.CodTipoDocumento)
                .HasColumnName("codtipodocumento");

            entity.Property(e => e.NumeroDocumento)
                .HasColumnName("numerodocumento")
                .HasMaxLength(55);

            entity.Property(e => e.NumeroTelefono)
                .HasColumnName("numerotelefono")
                .HasMaxLength(55);

            entity.Property(e => e.Correo)
                .HasColumnName("correo")
                .HasMaxLength(55);

            entity.Property(e => e.Direccion)
                .HasColumnName("direccion")
                .HasMaxLength(255);

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion

        #region"Producto"
        model.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.CodProducto);

            entity.Property(e => e.CodProducto)
                .HasColumnName("codproducto");

            entity.Property(e => e.NumProducto)
                .HasColumnName("numproducto")
                .HasMaxLength(55);

            entity.Property(e => e.CodigoBarra)
                .HasColumnName("codigobarra")
                .HasMaxLength(255);

            entity.Property(e => e.DesProducto)
                .HasColumnName("desproducto")
                .HasMaxLength(255);

            entity.Property(e => e.CodCategoria)
                .HasColumnName("codcategoria");

            entity.Property(e => e.CodUnidadMedida)
                .HasColumnName("codunidadmedida");

            entity.Property(e => e.CodIva)
                .HasColumnName("codiva");

            entity.Property(e => e.FechaIngreso)
                .HasColumnName("fechaingreso")
                .HasColumnType("datetime");

            entity.Property(e => e.CostoPromedio)
                .HasColumnName("costopromedio")
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.CostoUltimo)
                .HasColumnName("costoultimo")
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.DescuentaStock)
                .HasColumnName("descuentastock")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion

        #region"Categoria"
        model.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CodCategoria);

            entity.Property(e => e.CodCategoria)
                .HasColumnName("codcategoria");

            entity.Property(e => e.NumCategoria)
                .HasColumnName("numcategortia")
                .HasMaxLength(55);

            entity.Property(e => e.DesCategoria)
                .HasColumnName("descategoria")
                .HasMaxLength(255);

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion

        #region"UnidadMedida"
        model.Entity<UnidadMedida>(entity =>
        {
            entity.HasKey(e => e.CodUnidadMedida);

            entity.ToTable("unidadmedida");

            entity.Property(e => e.CodUnidadMedida)
                .HasColumnName("codunidadmedida");

            entity.Property(e => e.NumUnidadMedida)
                .HasColumnName("numunidadmedida")
                .HasMaxLength(55);

            entity.Property(e => e.DesUnidadMedida)
                .HasColumnName("desunidadmedida")
                .HasMaxLength(255);

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion

        #region"TipoPrecio"
        model.Entity<TipoPrecio>(entity =>
        {
            entity.HasKey(e => e.CodTipoPrecio);

            entity.Property(e => e.CodTipoPrecio)
                .HasColumnName("codtipoprecio");

            entity.Property(e => e.NumTipoPrecio)
                .HasColumnName("numtipoprecio")
                .HasMaxLength(55);

            entity.Property(e => e.DesTipoPrecio)
                .HasColumnName("destipoprecio")
                .HasMaxLength(255);

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion

        #region"Precio"
        model.Entity<Precio>(entity =>
        {
            entity.HasKey(e => e.CodPrecio);

            entity.Property(e => e.CodPrecio)
                .HasColumnName("codprecio");

            entity.Property(e => e.CodProducto)
                .HasColumnName("codproducto");

            entity.Property(e => e.CodTipoPrecio)
                .HasColumnName("codtipoprecio");

            entity.Property(e => e.PrecioVenta)
                .HasColumnName("precioventa")
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");

            entity.HasOne(e => e.TipoPrecio)
                .WithMany()
                .HasForeignKey(e => e.CodTipoPrecio)
                .OnDelete(DeleteBehavior.Restrict);
        });
        #endregion

        #region"Pedido"
        model.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.CodPedido);

            entity.Property(e => e.CodPedido)
                .HasColumnName("codpedido");

            entity.Property(e => e.NumPedido)
                .HasColumnName("numpedido")
                .HasMaxLength(55);

            entity.Property(e => e.Fecha)
                .HasColumnName("fecha")
                .HasColumnType("datetime");

            entity.Property(e => e.CodUsuario)
                .HasColumnName("codusuario");

            entity.Property(e => e.CodCliente)
                .HasColumnName("codcliente");

            entity.Property(e => e.CodMoneda)
                .HasColumnName("codmoneda");

            entity.Property(e => e.Total)
                .HasColumnName("total")
                .HasColumnType("numeric(18,2)")

            ;

            entity.Property(e => e.Iva)
                .HasColumnName("iva")
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.MotivoAnulacion)
                .HasColumnName("motivoanulacion")
                .HasMaxLength(255);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");

            entity.HasOne(e => e.Cliente)
                .WithMany()
                .HasForeignKey(e => e.CodCliente)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Detalles)
                .WithOne()
                .HasForeignKey(d => d.CodPedido)
                .OnDelete(DeleteBehavior.Cascade);
        });
        #endregion

        #region"PedidoDetalle"
        model.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.CodPedidoDetalle);

            entity.Property(e => e.CodPedidoDetalle)
                .HasColumnName("codpedidodetalle");

            entity.Property(e => e.CodPedido)
                .HasColumnName("codpedido");

            entity.Property(e => e.CodProducto)
                .HasColumnName("codproducto");

            entity.Property(e => e.Cantidad)
                .HasColumnName("cantidad")
                .HasColumnType("numeric(10,2)");

            entity.Property(e => e.PrecioUnitario)
                .HasColumnName("preciounitario")
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.LineaNumero)
                .HasColumnName("lineanumero");

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");

            entity.HasOne(e => e.Producto)
                .WithMany()
                .HasForeignKey(e => e.CodProducto)
                .OnDelete(DeleteBehavior.Restrict);
        });
        #endregion

        #region"Iva"
        model.Entity<Iva>(entity =>
        {
            entity.HasKey(e => e.CodIva);

            entity.Property(e => e.CodIva)
                .HasColumnName("codiva");

            entity.Property(e => e.NumIva)
                .HasColumnName("numiva")
                .HasMaxLength(55);

            entity.Property(e => e.DesIva)
                .HasColumnName("desiva")
                .HasMaxLength(255);

            entity.Property(e => e.Coeficiente)
                .HasColumnName("coeficiente")
                .HasColumnType("numeric(10,2)");

            entity.Property(e => e.Divisor)
                .HasColumnName("divisor");

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(true);

            entity.Property(e => e.FecGra)
                .HasColumnName("fecgra")
                .HasColumnType("datetime");
        });
        #endregion
    }
}
