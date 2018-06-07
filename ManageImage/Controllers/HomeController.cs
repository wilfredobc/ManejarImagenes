using ManageImage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ManageImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var ListImages = db.Images.ToList();
            return View(ListImages);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var image = db.Images.Find(id);

            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Images image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(image);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Images image)
        {
            try
            {
                //Obteniendo nombre del archivo
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);

                //Obteniendo extension
                string extension = Path.GetExtension(image.ImageFile.FileName);

                //concatenando fileName, fecha actual y extension
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                //Indicando donde se guardará el archivo
                image.ImagePath = $"~/Images/{fileName}";

                //Guardando imagen en el server
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                image.ImageFile.SaveAs(fileName);

                //Actualizando nombre en base de datos                
                db.Images.Add(image);
                db.SaveChanges();

                return RedirectToAction("Index");
                //ModelState.Clear(); <-- Ojo 
            }

            catch (Exception e) 
            {
                return View(image);
                //throw e;
            }            
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                        
            var image = db.Images.Find(id);             

            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var image = db.Images.Find(id);

            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            //Eliminar el registro de la base de datos
            var image = db.Images.Find(id);

            string pathToImage = Server.MapPath(image.ImagePath);

            if (System.IO.File.Exists(pathToImage))
            {
                try
                {
                     //Eliminar el archivo fisico en la ruta
                     System.IO.File.Delete(pathToImage);

                     //Eliminar registro de la base de datos
                     db.Images.Remove(image);
                     db.SaveChanges();
                     return RedirectToAction("Index");
                }
                catch (System.IO.IOException e)
                {
                    throw e;
                }
            } 
            return View();
        }
    }
}