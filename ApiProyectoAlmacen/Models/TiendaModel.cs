using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiProyectoAlmacen.Models
{
    public class TiendaModel
    {
        public int IdTienda { get; set; }
       
        public string Nombre { get; set; }
       
        public string Direccion { get; set; }
      
        public string Correo { get; set; }
    }
}
