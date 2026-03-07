using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class CitasController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var citas = db.Citas.Include(c => c.Pacientes).Include(c => c.Medicos);
            return View(citas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Citas citas = db.Citas.Find(id);
            if (citas == null) return HttpNotFound();
            return View(citas);
        }

        public ActionResult Create()
        {
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre");
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_cita,id_paciente,id_medico,fecha,hora,motivo_consulta,estado")] Citas citas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Citas.Add(citas);
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
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", citas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", citas.id_medico);
            return View(citas);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Citas citas = db.Citas.Find(id);
            if (citas == null) return HttpNotFound();
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", citas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", citas.id_medico);
            return View(citas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cita,id_paciente,id_medico,fecha,hora,motivo_consulta,estado")] Citas citas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(citas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", citas.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", citas.id_medico);
            return View(citas);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Citas citas = db.Citas.Find(id);
            if (citas == null) return HttpNotFound();
            return View(citas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Citas citas = db.Citas.Find(id);
            db.Citas.Remove(citas);
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
