using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {

        private RepositoryAlmacen repo;

        public ProductosController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetProductosBy/{idTienda}")]
        public async Task<ActionResult<List<Producto>>> GetProductosByIdTienda(int idTienda)
        {
            return await this.repo.GetProductosAsync(idTienda);
        }

        [HttpGet("FindProductoBy/{idProducto}")]
        public async Task<ActionResult<Producto>> FindProductoByIdProducto(int idProducto)
        {
            return await this.repo.FindProductoAsync(idProducto);
        }

        [HttpGet("SearchProductosBy/{idProducto}/{idTienda}")]
        public async Task<ActionResult<List<Producto>>> SearchProductosById(int idProducto, int idTienda)
        {
            return await this.repo.GetProductosByIdAsync(idProducto, idTienda);
        }

        [HttpGet("GetProductosByMarca/{marca}/{idTienda}")]
        public async Task<ActionResult<List<Producto>>> GetProductosByMarca(string marca, int idTienda)
        {
            return await this.repo.GetProductosByMarcaAsync(marca, idTienda);
        }

        [HttpPost]
        public async Task<ActionResult> InsertProducto(Producto p)
        {
            await this.repo.InsertProductoAsync(
                p.IdProducto,
                p.Nombre,
                p.Descripcion,
                p.Stock,
                p.Precio,
                p.Imagen,
                p.Marca,
                p.Modelo,
                p.IdProveedor,
                p.IdTienda
            );
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProducto(Producto p)
        {
            await this.repo.UpdateProductoAsync(p);
            return Ok();
        }

        [HttpPut("UpdateStock/{idProducto}/{nuevoStock}")]
        public async Task<ActionResult> UpdateProductoStock(int idProducto, int nuevoStock)
        {
            await this.repo.UpdateProductoStockAsync(idProducto, nuevoStock);
            return Ok();
        }
    }
}
