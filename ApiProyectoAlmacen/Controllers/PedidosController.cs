using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private RepositoryAlmacen repo;

        public PedidosController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetPedidosBy/{idTienda}")]
        public async Task<ActionResult<List<Pedido>>> GetPedidosByTienda(int idTienda)
        {
            return await repo.GetPedidosAsync(idTienda);
        }

        [HttpPost]
        public async Task<ActionResult> InsertPedido(Pedido p)
        {
            await repo.InsertPedidoAsync(p.IdPedido, p.IdProveedor, p.IdTienda, p.IdProducto, p.FechaPedido, p.FechaEntrega, p.Estado, p.Cantidad, p.Precio, p.PrecioTotalPedido);
            return Ok(new { message = "Pedido registrado correctamente" });
        }

        [HttpPut("UpdateEstadoPedido/{idPedido}/{nuevoEstado}")]
        public async Task<ActionResult> UpdateEstadoPedido(int idPedido, string nuevoEstado)
        {
            await repo.UpdateEstadoPedidoAsync(idPedido, nuevoEstado);
            return Ok(new { message = "Estado del pedido actualizado correctamente" });
        }
    }
}
