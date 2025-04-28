using ApiProyectoAlmacen.Models;
using Newtonsoft.Json;
using NugetProyectoAlmacen.Models;
using System.Security.Claims;

namespace ApiProyectoAlmacen.Helpers
{
    public class HelperUsuarioToken
    {
        private IHttpContextAccessor contextAccessor;

        public HelperUsuarioToken(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public TiendaModel GetUsuario()
        {
            Claim claim = this.contextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserData");
            string json = claim.Value;
            string jsonUsuario = HelperCryptography.DecryptString(json);
            TiendaModel model = JsonConvert.DeserializeObject<TiendaModel>(jsonUsuario);
            return model;
        }
    }
}
