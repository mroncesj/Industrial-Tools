using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Industrial_Tools.Models
{
    public class CarritoModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}