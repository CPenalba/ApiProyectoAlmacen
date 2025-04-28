using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiendasController : ControllerBase
    {
        private readonly RepositoryAlmacen repo;
        public TiendasController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [HttpGet("GetTiendas")]
        public async Task<ActionResult<List<Tienda>>> GetTiendas()
        {
            return await repo.GetTiendasAsync();
        }

        [HttpGet("GetTiendaById/{tiendaId}")]
        public async Task<ActionResult<Tienda>> GetTiendaById(int tiendaId)
        {
            return await repo.GetTiendaByIdAsync(tiendaId);
        }

        [HttpGet("GetTiendaByCorreo/{correo}")]
        public async Task<ActionResult<Tienda>> GetTiendaByCorreo(string correo)
        {
            return await this.repo.GetTiendaByCorreo(correo);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Tienda>> Login(Tienda t)
        {
            try
            {
                var tienda = await repo.LoginAsync(t.Correo, t.Contraseña);

                if (tienda == null)
                {
                    return Unauthorized("Credenciales incorrectas");
                }

                return Ok(tienda);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost("insertar")]
        public async Task<ActionResult> CreateTienda(Tienda t)
        {
            await repo.InsertTiendaAsync(t.Nombre, t.Direccion, t.Correo, t.Contraseña);
            return Ok(new { message = "Tienda registrada correctamente" });
        }
    }
}
