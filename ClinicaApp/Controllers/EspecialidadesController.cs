using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class EspecialidadesController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            return View(db.Especialidades.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null) return HttpNotFound();
            return View(especialidades);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_especialidad,nombre_especialidad,descripcion")] Especialidades especialidades)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Especialidades.Add(especialidades);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                    foreach (var ve in error.ValidationErrors)
                        ModelState.AddModelError("", "Campo: " + ve.PropertyName + " - " + ve.ErrorMessage);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            return View(especialidades);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null) return HttpNotFound();
            return View(especialidades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_especialidad,nombre_especialidad,descripcion")] Especialidades especialidades)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(especialidades).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            return View(especialidades);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Especialidades especialidades = db.Especialidades.Find(id);
            if (especialidades == null) return HttpNotFound();
            return View(especialidades);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especialidades especialidades = db.Especialidades.Find(id);
            db.Especialidades.Remove(especialidades);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}