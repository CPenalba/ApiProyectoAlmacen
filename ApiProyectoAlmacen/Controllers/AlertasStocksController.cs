using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertasStocksController : ControllerBase
    {
        private RepositoryAlmacen repo;

        public AlertasStocksController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetAlertasStocksBy/{idTienda}")]
        public async Task<ActionResult<List<AlertaStock>>> GetAlertasStocksByIdTienda(int idTienda)
        {
            try
            {
                return await this.repo.GetAlertasStocksAsync(idTienda);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener alertas de stock: {ex.Message}");
            }
        }

        [HttpGet("FindAlertaBy/{idAlerta}")]

        public async Task<ActionResult<AlertaStock>> FindAlertaByIdAlerta(int idAlerta)
        {
            return await this.repo.FindAlertaAsync(idAlerta);
        }

        [HttpGet("GetProductoBy/{idProducto}")]

        public async Task<ActionResult<Producto>> GetProductoByIdProducto(int idProducto)
        {
            try
            {
                var producto = await this.repo.GetProductoByIdAsync(idProducto);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {idProducto} no encontrado");
                }
                return producto;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener producto: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertAlertaAsync(AlertaStock a)
        {
            await this.repo.InsertAlertaAsync(a.IdAlertaStock, a.IdProducto, a.IdTienda, a.FechaAlerta, a.Descripcion, a.Estado);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlertaById(int id)
        {
            await this.repo.DeleteAlertaAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAlerta(AlertaStock a)
        {
            await this.repo.UpdateAlertaAsync(a);
            return Ok();
        }
    }
}
