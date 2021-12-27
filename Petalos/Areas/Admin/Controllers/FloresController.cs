using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Petalos.Models;
using Petalos.Areas.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;

//ADMIN

namespace Petalos.Controllers
{
    [Area("Admin")]
    public class FloresController : Controller
    {
        public floresContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public FloresController(floresContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }
        public IActionResult Index()
        {
            var f = Context.Datosflores.OrderBy(x => x.Nombre);
            return View(f);
        }
        [HttpGet]
        public IActionResult AgregarFlor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AgregarFlor(FloresCrud ViewModelFlores)
        {
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Nombre))
            {
                ModelState.AddModelError("", "El nombre de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Nombrecientifico))
            {
                ModelState.AddModelError("", "El nombre cientifico de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Origen))
            {
                ModelState.AddModelError("", "El origen de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Descripcion))
            {
                ModelState.AddModelError("", "La descripción de la flor está vacía");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombre == ViewModelFlores.Flor.Nombre))
            {
                ModelState.AddModelError("", "El nombre de la flor ya existe");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombrecientifico == ViewModelFlores.Flor.Nombrecientifico))
            {
                ModelState.AddModelError("", "El nombre científico de la flor ya existe");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombrecomun == ViewModelFlores.Flor.Nombrecomun))
            {
                ModelState.AddModelError("", "El nombre común de la flor ya existe");
                return View(ViewModelFlores);
            }
            Context.Add(ViewModelFlores.Flor);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AgregarImagenes(int id)
        {
            var f = Context.Datosflores.Include(x => x.Imagenesflores).FirstOrDefault(x => x.Idflor == id);
            if (f == null)
            {
                return RedirectToAction("Index");
            }
            FloresCrud vm = new();
            vm.ImagenId = Context.Imagenesflores.FirstOrDefault(x => x.Idflor == f.Idflor);
            vm.Flor = f;
            vm.Flores = Context.Datosflores.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult AgregarImagenes(FloresCrud ViewModelFlores,IFormFile foto)
        {
            if (foto==null)
            {
                ViewModelFlores.Flores = Context.Datosflores.OrderBy(x => x.Nombre);
                ModelState.AddModelError("", "No hay ninguna fotografia");
                return View(ViewModelFlores);
            }
            if (foto != null)
            {
                if (foto.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "Solo se permite la carga de archivos JPG");
                    return View(ViewModelFlores);
                }
                if (foto.Length > 1024 * 1024 * 5)
                {
                    ModelState.AddModelError("", "No se permite la carga de archivos mayores a 5MB");
                    return View(ViewModelFlores);
                }
            }
            ViewModelFlores.ImagenId.Nombreimagen = foto.FileName;
            Context.Add(ViewModelFlores.ImagenId);
            Context.SaveChanges();

            if (foto != null)
            {
                String var = "Hola Mundo";
                int tam_var = var.Length;
                String Var_Sub = var.Substring((tam_var - 2), 2);
                var path = Host.WebRootPath + "/images/" + ViewModelFlores.ImagenId.Nombreimagen;
                FileStream fs = new FileStream(path, FileMode.Create);
                foto.CopyTo(fs);
                fs.Close();
            }
            return RedirectToAction("AgregarImagenes");
        }

        [HttpGet]
        public IActionResult EditarFlor(int id)
        {
            var f = Context.Datosflores.FirstOrDefault(x => x.Idflor == id);
            if (f == null)
            {
                return RedirectToAction("Index");
            }
            FloresCrud vm = new FloresCrud
            {
                Flor = f
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult EditarFlor(FloresCrud ViewModelFlores)
        {

            var f = Context.Datosflores.FirstOrDefault(x => x.Idflor == ViewModelFlores.Flor.Idflor);
            if (f == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Nombre))
            {
                ModelState.AddModelError("", "El nombre de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Nombrecientifico))
            {
                ModelState.AddModelError("", "El nombre cientifico de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Origen))
            {
                ModelState.AddModelError("", "El origen de la flor está vacío");
                return View(ViewModelFlores);
            }
            if (string.IsNullOrWhiteSpace(ViewModelFlores.Flor.Descripcion))
            {
                ModelState.AddModelError("", "La descripción de la flor está vacía");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombre == ViewModelFlores.Flor.Nombre && x.Idflor != ViewModelFlores.Flor.Idflor))
            {
                ModelState.AddModelError("", "El nombre de la flor ya existe");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombrecientifico == ViewModelFlores.Flor.Nombrecientifico && x.Idflor != ViewModelFlores.Flor.Idflor))
            {
                ModelState.AddModelError("", "El nombre científico de la flor ya existe");
                return View(ViewModelFlores);
            }
            if (Context.Datosflores.Any(x => x.Nombrecomun == ViewModelFlores.Flor.Nombrecomun && x.Idflor != ViewModelFlores.Flor.Idflor))
            {
                ModelState.AddModelError("", "El nombre común de la flor ya existe");
                return View(ViewModelFlores);
            }
            f.Nombrecientifico = ViewModelFlores.Flor.Nombrecientifico;
            f.Nombrecomun = ViewModelFlores.Flor.Nombrecomun;
            f.Origen = ViewModelFlores.Flor.Origen;
            f.Descripcion = ViewModelFlores.Flor.Descripcion;
            f.Nombre = ViewModelFlores.Flor.Nombre;
            Context.Update(f);
            Context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult EliminarFlor(int id)
        {
            var f = Context.Datosflores.FirstOrDefault(x => x.Idflor == id);
            if (f == null)
            {
                return RedirectToAction("Index");
            }
            return View(f);
        }

        [HttpGet]
        public IActionResult EliminarImagen(int id)
        {
            var img = Context.Imagenesflores.FirstOrDefault(x => x.Idimagen == id);
            if (img == null)
            {
                return RedirectToAction("Index");
            }
            return View(img);
        }

        [HttpPost]
        public IActionResult EliminarImagen(Imagenesflore imagenF)
        {
            var img = Context.Imagenesflores.FirstOrDefault(x => x.Idimagen == imagenF.Idimagen);

            if (img == null)
            {
                ModelState.AddModelError("", $"La fotografía {imagenF.Nombreimagen} ha sido eliminada, o no existe");
                return View(imagenF);
            }
            Context.Remove(img);
            Context.SaveChanges();
            string path = Host.WebRootPath + "/images/" + img.Nombreimagen;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EliminarFlor(Datosflore flor)
        {
            var f = Context.Datosflores.FirstOrDefault(x => x.Idflor == flor.Idflor);
            if (f == null)
            {
                ModelState.AddModelError("", "La flor ya ha sido eliminada o no existe");
                return View(flor);
            }
            Context.Remove(f);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

    }


}