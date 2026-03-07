using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class RecetasController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var recetas = db.Recetas.Include(r => r.Pacientes).Include(r => r.Medicos);
            return View(recetas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Recetas recetas = db.Recetas.Find(id);
            if (recetas == null) return HttpNotFound();
            return View(recetas);
        }

        public ActionResult Create()
        {
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre");
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_receta,id_paciente,id_medico,fecha,indicaciones")] Recetas recetas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Recetas.Add(recetas);
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
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", recetas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", recetas.id_medico);
            return View(recetas);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Recetas recetas = db.Recetas.Find(id);
            if (recetas == null) return HttpNotFound();
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", recetas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", recetas.id_medico);
            return View(recetas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_receta,id_paciente,id_medico,fecha,indicaciones")] Recetas recetas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(recetas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", recetas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", recetas.id_medico);
            return View(recetas);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Recetas recetas = db.Recetas.Find(id);
            if (recetas == null) return HttpNotFound();
            return View(recetas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recetas recetas = db.Recetas.Find(id);
            db.Recetas.Remove(recetas);
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