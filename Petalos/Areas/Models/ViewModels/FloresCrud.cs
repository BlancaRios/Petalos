using Microsoft.AspNetCore.Http;
using Petalos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Petalos.Areas.Models.ViewModels
{
    public class FloresCrud
    {
        public Datosflore Flor { get; set; }
        public IEnumerable<Datosflore> Flores { get; set; }
        public Imagenesflore ImagenId { get; set; }
       

    }
}
