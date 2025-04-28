using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private RepositoryAlmacen repo;

        public ProveedoresController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet("GetProveedores")]
        public async Task<ActionResult<List<Proveedor>>> GetProveedores()
        {
            return await repo.GetProveedoresAsync();
        }

        [Authorize]
        [HttpGet("GetProveedorById/{idProveedor}")]
        public async Task<ActionResult<Proveedor>> GetProveedorById(int idProveedor)
        {
            return await repo.FindProveedorAsync(idProveedor);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateProveedor(Proveedor p)
        {
            await repo.InsertProveedorAsync(p.IdProveedor, p.Nombre, p.Telefono, p.Correo, p.Direccion);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateProveedor(Proveedor proveedor)
        {
            await repo.UpdateProveedorAsync(proveedor);
            return Ok();
        }
    }
}
