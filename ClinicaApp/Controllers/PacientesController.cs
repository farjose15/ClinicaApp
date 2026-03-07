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
    public class PacientesController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            return View(db.Pacientes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Pacientes pacientes = db.Pacientes.Find(id);
            if (pacientes == null) return HttpNotFound();
            return View(pacientes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_paciente,nombre,apellido,fecha_nacimiento,sexo,ci,direccion,telefono,correo,tipo_sangre,alergias,contacto_emergencia")] Pacientes pacientes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Pacientes.Add(pacientes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var errores = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errores)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var ve in error.ValidationErrors)
                    {
                        ModelState.AddModelError("", "Campo: " + ve.PropertyName + " - " + ve.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error al guardar: " + e.Message);
            }
            return View(pacientes);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Pacientes pacientes = db.Pacientes.Find(id);
            if (pacientes == null) return HttpNotFound();
            return View(pacientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_paciente,nombre,apellido,fecha_nacimiento,sexo,ci,direccion,telefono,correo,tipo_sangre,alergias,contacto_emergencia")] Pacientes pacientes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(pacientes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error al guardar: " + e.Message);
            }
            return View(pacientes);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Pacientes pacientes = db.Pacientes.Find(id);
            if (pacientes == null) return HttpNotFound();
            return View(pacientes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pacientes pacientes = db.Pacientes.Find(id);
            db.Pacientes.Remove(pacientes);
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