using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class MedicamentosController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            return View(db.Medicamentos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicamentos medicamentos = db.Medicamentos.Find(id);
            if (medicamentos == null) return HttpNotFound();
            return View(medicamentos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_medicamento,nombre_medicamento,descripcion,dosis,stock,precio")] Medicamentos medicamentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Medicamentos.Add(medicamentos);
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
            return View(medicamentos);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicamentos medicamentos = db.Medicamentos.Find(id);
            if (medicamentos == null) return HttpNotFound();
            return View(medicamentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_medicamento,nombre_medicamento,descripcion,dosis,stock,precio")] Medicamentos medicamentos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medicamentos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            return View(medicamentos);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Medicamentos medicamentos = db.Medicamentos.Find(id);
            if (medicamentos == null) return HttpNotFound();
            return View(medicamentos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicamentos medicamentos = db.Medicamentos.Find(id);
            db.Medicamentos.Remove(medicamentos);
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