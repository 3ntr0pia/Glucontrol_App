using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAplicacion.DTOs
{
    public class DTOLoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
    }
}
