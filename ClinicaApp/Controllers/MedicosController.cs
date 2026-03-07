using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class MedicosController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var medicos = db.Medicos.Include(m => m.Especialidades);
            return View(medicos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicos medicos = db.Medicos.Find(id);
            if (medicos == null) return HttpNotFound();
            return View(medicos);
        }

        public ActionResult Create()
        {
            ViewBag.id_especialidad = new SelectList(db.Especialidades, "id_especialidad", "nombre_especialidad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_medico,nombre,apellido,telefono,correo,matricula,id_especialidad")] Medicos medicos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Medicos.Add(medicos);
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
            ViewBag.id_especialidad = new SelectList(db.Especialidades, "id_especialidad", "nombre_especialidad", medicos.id_especialidad);
            return View(medicos);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicos medicos = db.Medicos.Find(id);
            if (medicos == null) return HttpNotFound();
            ViewBag.id_especialidad = new SelectList(db.Especialidades, "id_especialidad", "nombre_especialidad", medicos.id_especialidad);
            return View(medicos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_medico,nombre,apellido,telefono,correo,matricula,id_especialidad")] Medicos medicos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medicos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_especialidad = new SelectList(db.Especialidades, "id_especialidad", "nombre_especialidad", medicos.id_especialidad);
            return View(medicos);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicos medicos = db.Medicos.Find(id);
            if (medicos == null) return HttpNotFound();
            return View(medicos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicos medicos = db.Medicos.Find(id);
            db.Medicos.Remove(medicos);
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