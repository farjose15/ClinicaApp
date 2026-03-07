using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class DetalleRecetaController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var detalle = db.Detalle_Receta.Include(d => d.Recetas).Include(d => d.Medicamentos);
            return View(detalle.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Detalle_Receta detalle = db.Detalle_Receta.Find(id);
            if (detalle == null) return HttpNotFound();
            return View(detalle);
        }

        public ActionResult Create()
        {
            ViewBag.id_receta = new SelectList(db.Recetas, "id_receta", "id_receta");
            ViewBag.id_medicamento = new SelectList(db.Medicamentos, "id_medicamento", "nombre_medicamento");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_detalle,id_receta,id_medicamento,cantidad")] Detalle_Receta detalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Detalle_Receta.Add(detalle);
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
            ViewBag.id_receta = new SelectList(db.Recetas, "id_receta", "id_receta", detalle.id_receta);
            ViewBag.id_medicamento = new SelectList(db.Medicamentos, "id_medicamento", "nombre_medicamento", detalle.id_medicamento);
            return View(detalle);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Detalle_Receta detalle = db.Detalle_Receta.Find(id);
            if (detalle == null) return HttpNotFound();
            ViewBag.id_receta = new SelectList(db.Recetas, "id_receta", "id_receta", detalle.id_receta);
            ViewBag.id_medicamento = new SelectList(db.Medicamentos, "id_medicamento", "nombre_medicamento", detalle.id_medicamento);
            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_detalle,id_receta,id_medicamento,cantidad")] Detalle_Receta detalle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(detalle).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_receta = new SelectList(db.Recetas, "id_receta", "id_receta", detalle.id_receta);
            ViewBag.id_medicamento = new SelectList(db.Medicamentos, "id_medicamento", "nombre_medicamento", detalle.id_medicamento);
            return View(detalle);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Detalle_Receta detalle = db.Detalle_Receta.Find(id);
            if (detalle == null) return HttpNotFound();
            return View(detalle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Detalle_Receta detalle = db.Detalle_Receta.Find(id);
            db.Detalle_Receta.Remove(detalle);
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