using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallesVentasController : ControllerBase
    {
        private RepositoryAlmacen repo;

        public DetallesVentasController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet("GetDetallesVentas/{idTienda}")]
        public async Task<ActionResult<List<DetalleVenta>>> GetDetallesVentasByTienda(int idTienda)
        {
            return await repo.GetDetallesVentasAsync(idTienda);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> InsertVenta(DetalleVenta v)
        {
            await repo.InsertVentaAsync(v.IdDetalleVenta, v.Fecha, v.IdProducto, v.IdTienda, v.Cantidad, v.Precio, v.PrecioTotalVenta);
            return Ok(new { message = "Venta registrada correctamente" });
        }
        [Authorize]
        [HttpGet("GetAniosVentas/{tiendaId}")]
        public async Task<ActionResult<List<int>>> GetAniosVentas(int tiendaId)
        {
            return await repo.GetAñosVentasAsync(tiendaId);
        }
        [Authorize]
        [HttpGet("GetVentasPorAnio{tiendaId}/{año}")]
        public async Task<ActionResult<Dictionary<string, int>>> GetVentasPorAnio(int tiendaId, int año)
        {
            return await repo.GetVentasPorMesAsync(tiendaId, año);
        }

        [Authorize]
        [HttpGet("GetProductosMasVendidos/{tiendaId}")]
        public async Task<ActionResult<List<Producto>>> GetProductosMasVendidos(int tiendaId, [FromQuery] int top = 4)
        {
            return await repo.GetProductosMasVendidosAsync(tiendaId, top);
        }
    }
}
