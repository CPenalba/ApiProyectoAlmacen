using ApiProyectoAlmacen.Data;
using Microsoft.EntityFrameworkCore;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Repositories
{
    public class RepositoryAlmacen
    {
        private AlmacenContext context;

        public RepositoryAlmacen(AlmacenContext context)
        {
            this.context = context;
        }

        public async Task<List<AlertaStock>> GetAlertasStocksAsync(int idTienda)
        {
            return await this.context.AlertasStocks
                         .Where(p => p.IdTienda == idTienda)
                         .ToListAsync();
        }

        public async Task<AlertaStock> FindAlertaAsync(int idAlerta)
        {
            var consulta = from datos in this.context.AlertasStocks where datos.IdAlertaStock == idAlerta select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task InsertAlertaAsync(int idAlerta, int idProducto, int idTienda, DateTime fechaAlerta, string descripcion, string estado)
        {
            int maxId = (await this.context.AlertasStocks.MaxAsync(t => (int?)t.IdAlertaStock) ?? 0) + 1;
            AlertaStock a = new AlertaStock();
            a.IdAlertaStock = maxId;
            a.IdProducto = idProducto;
            a.IdTienda = idTienda;
            a.FechaAlerta = fechaAlerta;
            a.Descripcion = descripcion;
            a.Estado = estado;
            await this.context.AlertasStocks.AddAsync(a);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAlertaAsync(AlertaStock a)
        {
            this.context.AlertasStocks.Update(a);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAlertaAsync(int idAlertaStock)
        {
            AlertaStock a = await this.FindAlertaAsync(idAlertaStock);
            this.context.AlertasStocks.Remove(a);
            await this.context.SaveChangesAsync();
        }

        public async Task<Producto> GetProductoByIdAsync(int idProducto)
        {
            return await this.context.Productos
                                     .Where(p => p.IdProducto == idProducto)
                                     .FirstOrDefaultAsync();
        }

        public async Task<List<DetalleVenta>> GetDetallesVentasAsync(int idTienda)
        {
            return await this.context.DetallesVentas
                        .Where(p => p.IdTienda == idTienda)
                         .Include(p => p.Producto)
                        .ToListAsync();
        }

        public async Task InsertVentaAsync(int idVenta, DateTime fecha, int idProducto, int idTienda, int cantidad, decimal precio, decimal precioTotalVenta)
        {
            DetalleVenta v = new DetalleVenta();
            v.IdDetalleVenta = idVenta;
            v.Fecha = fecha;
            v.IdProducto = idProducto;
            v.IdTienda = idTienda;
            v.Cantidad = cantidad;
            v.Precio = precio;
            v.PrecioTotalVenta = precioTotalVenta;
            await this.context.DetallesVentas.AddAsync(v);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<int>> GetAñosVentasAsync(int tiendaId)
        {
            return await context.DetallesVentas
                .Where(v => v.IdTienda == tiendaId)
                .Select(v => v.Fecha.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetVentasPorMesAsync(int tiendaId, int año)
        {
            var ventasPorMes = await context.DetallesVentas
                .Where(v => v.IdTienda == tiendaId && v.Fecha.Year == año)
                .GroupBy(v => new { v.Fecha.Month })
                .Select(g => new
                {
                    Mes = g.Key.Month,
                    TotalVentas = g.Sum(v => v.Cantidad)
                })
                .OrderBy(x => x.Mes)
                .ToListAsync();

            var meses = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var resultado = new Dictionary<string, int>();
            foreach (var item in ventasPorMes)
            {
                resultado[meses[item.Mes - 1]] = item.TotalVentas;
            }
            for (int i = 1; i <= 12; i++)
            {
                var nombreMes = meses[i - 1];
                if (!resultado.ContainsKey(nombreMes))
                {
                    resultado[nombreMes] = 0;
                }
            }

            return resultado.OrderBy(x => Array.IndexOf(meses, x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        public async Task<List<Producto>> GetProductosMasVendidosAsync(int tiendaId, int top = 4)
        {
            return await context.DetallesVentas
                .Where(v => v.IdTienda == tiendaId)
                .GroupBy(v => v.IdProducto)
                .OrderByDescending(g => g.Sum(v => v.Cantidad))
                .Take(top)
                .Select(g => g.FirstOrDefault().Producto)
                .ToListAsync();
        }

        public async Task<List<Pedido>> GetPedidosAsync(int idTienda)
        {
            return await this.context.Pedidos
                         .Where(p => p.IdTienda == idTienda)
                         .Include(p => p.Proveedor)
                         .Include(p => p.Producto)
                         .ToListAsync();
        }

        public async Task InsertPedidoAsync(int idPedido, int idProveedor, int idTienda, int idProducto, DateTime fechaPedido, DateTime fechaEntrega, string estado, int cantidad, decimal precio, decimal precioTotalPedido)
        {
            Pedido p = new Pedido();
            p.IdPedido = idPedido;
            p.IdProveedor = idProveedor;
            p.IdTienda = idTienda;
            p.IdProducto = idProducto;
            p.FechaPedido = fechaPedido;
            p.FechaEntrega = fechaEntrega;
            p.Estado = estado;
            p.Cantidad = cantidad;
            p.Precio = precio;
            p.PrecioTotalPedido = precioTotalPedido;
            await this.context.Pedidos.AddAsync(p);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateEstadoPedidoAsync(int idPedido, string nuevoEstado)
        {
            var pedido = await this.context.Pedidos.FindAsync(idPedido);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<Producto>> GetProductosAsync(int idTienda)
        {
            return await this.context.Productos
                         .Where(p => p.IdTienda == idTienda)
                         .Include(p => p.Proveedor)
                         .Include(p => p.Tienda)
                         .ToListAsync();
        }

        public async Task InsertProductoAsync(int idProducto, string nombre, string descripcion, int stock, decimal precio, string? imagen, string marca, string modelo, int idProveedor, int idTienda)
        {
            Producto p = new Producto();
            p.IdProducto = idProducto;
            p.Nombre = nombre;
            p.Descripcion = descripcion;
            p.Stock = stock;
            p.Precio = precio;
            p.Imagen = imagen;
            p.Marca = marca;
            p.Modelo = modelo;
            p.IdProveedor = idProveedor;
            p.IdTienda = idTienda;
            await this.context.Productos.AddAsync(p);
            await this.context.SaveChangesAsync();
        }

        public async Task<Producto> FindProductoAsync(int idProducto)
        {
            return await this.context.Productos
                             .Include(p => p.Proveedor)
                             .FirstOrDefaultAsync(p => p.IdProducto == idProducto);
        }

        public async Task<List<Producto>> GetProductosByIdAsync(int idProducto, int idTienda)
        {
            return await this.context.Productos
                         .Where(p => p.IdProducto.ToString().StartsWith(idProducto.ToString()) && p.IdTienda == idTienda)
                         .Include(p => p.Proveedor)
                         .Include(p => p.Tienda)
                         .ToListAsync();
        }

        public async Task<List<Producto>> GetProductosByMarcaAsync(string marca, int idTienda)
        {
            return await this.context.Productos
                         .Where(p => p.Marca.StartsWith(marca) && p.IdTienda == idTienda)
                         .Include(p => p.Proveedor)
                         .Include(p => p.Tienda)
                         .ToListAsync();
        }

        public async Task UpdateProductoStockAsync(int idProducto, int nuevoStock)
        {
            var producto = await context.Productos.FindAsync(idProducto);
            if (producto != null)
            {
                producto.Stock = nuevoStock;
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            this.context.Productos.Update(producto);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Proveedor>> GetProveedoresAsync()
        {
            return await this.context.Proveedores.ToListAsync();
        }

        public async Task<Proveedor> FindProveedorAsync(int idProveedor)
        {
            return await this.context.Proveedores.FirstOrDefaultAsync(p => p.IdProveedor == idProveedor);
        }

        public async Task InsertProveedorAsync(int idProveedor, string nombre, string telefono, string correo, string direccion)
        {
            int maxId = (await this.context.Proveedores.MaxAsync(p => (int?)p.IdProveedor) ?? 0) + 1;
            Proveedor p = new Proveedor();
            p.IdProveedor = maxId;
            p.Nombre = nombre;
            p.Telefono = telefono;
            p.Correo = correo;
            p.Direccion = direccion;
            await this.context.Proveedores.AddAsync(p);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateProveedorAsync(Proveedor p)
        {
            this.context.Proveedores.Update(p);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Tienda>> GetTiendasAsync()
        {
            var consulta = from datos in this.context.Tiendas select datos;
            return await consulta.ToListAsync();
        }

        public async Task<Tienda> LoginAsync(string correo, string contraseña)
        {
            var tienda = await this.context.Tiendas.FirstOrDefaultAsync(t => t.Correo == correo);
            if (tienda != null && BCrypt.Net.BCrypt.Verify(contraseña, tienda.Contraseña))
            {
                return tienda;
            }
            return null;
        }

        public async Task<Tienda> GetTiendaByIdAsync(int tiendaId)
        {
            return await this.context.Tiendas.FirstOrDefaultAsync(t => t.IdTienda == tiendaId);
        }

        public string HashPwd(string contraseña)
        {
            return BCrypt.Net.BCrypt.HashPassword(contraseña);
        }


        public async Task InsertTiendaAsync(string nombre, string direccion, string correo, string contraseña)
        {
            int maxId = (await this.context.Tiendas.MaxAsync(t => (int?)t.IdTienda) ?? 0) + 1;
            Tienda t = new Tienda();
            t.IdTienda = maxId;
            t.Nombre = nombre;
            t.Direccion = direccion;
            t.Correo = correo;
            t.Contraseña = this.HashPwd(contraseña);
            await this.context.Tiendas.AddAsync(t);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> GetUsuariosAsync(int idTienda)
        {
            return await this.context.Usuarios.Include(p => p.Tienda).Where(p => p.IdTienda == idTienda).ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int idUsuario)
        {

            return await this.context.Usuarios
                .Include(u => u.Tienda)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }
        public async Task<bool> CambiarContraseñaAsync(int idUsuario, string nuevaContraseña)
        {
            var usuario = await this.GetUsuarioByIdAsync(idUsuario);
            if (usuario == null)
                return false;
            string hash = BCrypt.Net.BCrypt.HashPassword(nuevaContraseña);
            usuario.Contraseña = hash;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            this.context.Usuarios.Update(usuario);
            await this.context.SaveChangesAsync();
        }

        public async Task InsertUsuarioAsync(int id, string nombre, string imagen, string correo, string contraseña, string rol, int idTienda)
        {
            Usuario u = new Usuario();
            u.IdUsuario = id;
            u.Nombre = nombre;
            u.Imagen = imagen;
            u.Correo = correo;
            u.Contraseña = BCrypt.Net.BCrypt.HashPassword(contraseña);
            u.Rol = rol;
            u.IdTienda = idTienda;
            await this.context.Usuarios.AddAsync(u);
            await this.context.SaveChangesAsync();
        }
    }
}
