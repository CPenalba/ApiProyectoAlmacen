using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetProyectoAlmacen.Models;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly RepositoryAlmacen repo;

        public UsuariosController(RepositoryAlmacen repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet("GetUsuariosBy/{idTienda}")]
        public async Task<ActionResult<List<Usuario>>> GetUsuariosByTienda(int idTienda)
        {
            var usuarios = await repo.GetUsuariosAsync(idTienda);
            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("GetUsuarioBy/{idUsuario}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByIdUsuario(int idUsuario)
        {
            var usuario = await repo.GetUsuarioByIdAsync(idUsuario);
            return Ok(usuario);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateUsuario(Usuario u)
        {
            await repo.InsertUsuarioAsync(u.IdUsuario, u.Nombre, u.Imagen, u.Correo, u.Contraseña, u.Rol, u.IdTienda);
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUsuario(Usuario usuario)
        {
            await repo.UpdateUsuarioAsync(usuario);
            return Ok(new { message = "Usuario actualizado correctamente" });
        }

        [Authorize]
        [HttpPut("UpdateUsuario/{idUsuario}/{nuevaContraseña}")]
        public async Task<IActionResult> CambiarContraseña(int idUsuario, string nuevaContraseña)
        {
            await repo.CambiarContraseñaAsync(idUsuario, nuevaContraseña);
            return Ok(new { message = "Contraseña actualizada correctamente" });
        }
    }
}
