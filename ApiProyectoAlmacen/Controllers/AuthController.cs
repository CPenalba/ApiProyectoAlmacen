using ApiProyectoAlmacen.Helpers;
using ApiProyectoAlmacen.Models;
using ApiProyectoAlmacen.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NugetProyectoAlmacen.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiProyectoAlmacen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryAlmacen repo;
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryAlmacen repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Tienda t = await this.repo.LoginAsync(model.Email, model.Pass);
            if (t == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                TiendaModel modeUsuario = new TiendaModel();
                modeUsuario.IdTienda = t.IdTienda;
                modeUsuario.Nombre = t.Nombre;
                modeUsuario.Direccion = t.Direccion;
                modeUsuario.Correo = t.Correo;

                string jsonUsuario = JsonConvert.SerializeObject(modeUsuario);
                string jsonCrifado = HelperCryptography.EncryptString(jsonUsuario);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonCrifado)
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(20),
                    notBefore: DateTime.UtcNow
                    );
                //POR ULTIMO DEVOLVEMOS LA RESPUESTA AFIRMATIVA CON UN OBJETO QUE CONTENGA EL TOKEN (ANONIMO)
                return Ok(
                    new
                    {
                        response = new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }
    }
}
