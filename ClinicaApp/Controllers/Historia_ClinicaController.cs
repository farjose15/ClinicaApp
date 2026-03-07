using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class HistoriaClinicaController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var historia = db.Historia_Clinica.Include(h => h.Pacientes).Include(h => h.Medicos);
            return View(historia.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Historia_Clinica historia = db.Historia_Clinica.Find(id);
            if (historia == null) return HttpNotFound();
            return View(historia);
        }

        public ActionResult Create()
        {
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre");
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_historia,id_paciente,id_medico,fecha_consulta,sintomas,diagnostico,tratamiento,observaciones")] Historia_Clinica historia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Historia_Clinica.Add(historia);
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
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", historia.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", historia.id_medico);
            return View(historia);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Historia_Clinica historia = db.Historia_Clinica.Find(id);
            if (historia == null) return HttpNotFound();
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", historia.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", historia.id_medico);
            return View(historia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_historia,id_paciente,id_medico,fecha_consulta,sintomas,diagnostico,tratamiento,observaciones")] Historia_Clinica historia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(historia).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", historia.id_paciente);
            ViewBag.id_medico = new SelectList(db.Medicos, "id_medico", "nombre", historia.id_medico);
            return View(historia);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Historia_Clinica historia = db.Historia_Clinica.Find(id);
            if (historia == null) return HttpNotFound();
            return View(historia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Historia_Clinica historia = db.Historia_Clinica.Find(id);
            db.Historia_Clinica.Remove(historia);
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