using Microsoft.EntityFrameworkCore;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Data
{
    public class AlmacenContext : DbContext
    {
        public AlmacenContext(DbContextOptions<AlmacenContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }
        public DbSet<AlertaStock> AlertasStocks { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany()
                .HasForeignKey(p => p.IdProveedor);

            modelBuilder.Entity<Producto>()
        .HasOne(p => p.Tienda)
        .WithMany()
        .HasForeignKey(p => p.IdTienda);

            modelBuilder.Entity<Usuario>()
               .HasOne(p => p.Tienda)
               .WithMany()
               .HasForeignKey(p => p.IdTienda);


            base.OnModelCreating(modelBuilder);
        }
    }
}
