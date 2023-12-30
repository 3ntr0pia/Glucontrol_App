using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaAplicacion.DTOs
{
    public class DTOPersonaRegistro
    {
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public int Edad { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string TipoDiabetes { get; set; }
        public int UserId { get; set; }
    }
}
