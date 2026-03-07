using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ClinicaApp.Models;

namespace ClinicaApp.Controllers
{
    public class FacturasController : Controller
    {
        private ClinicaEntities db = new ClinicaEntities();

        public ActionResult Index()
        {
            var facturas = db.Facturas.Include(f => f.Pacientes);
            return View(facturas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null) return HttpNotFound();
            return View(facturas);
        }

        public ActionResult Create()
        {
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_factura,id_paciente,fecha,servicio,monto,metodo_pago")] Facturas facturas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Facturas.Add(facturas);
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
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", facturas.id_paciente);
            return View(facturas);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null) return HttpNotFound();
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", facturas.id_paciente);
            return View(facturas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_factura,id_paciente,fecha,servicio,monto,metodo_pago")] Facturas facturas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(facturas).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error: " + e.Message);
            }
            ViewBag.id_paciente = new SelectList(db.Pacientes, "id_paciente", "nombre", facturas.id_paciente);
            return View(facturas);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Facturas facturas = db.Facturas.Find(id);
            if (facturas == null) return HttpNotFound();
            return View(facturas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Facturas facturas = db.Facturas.Find(id);
            db.Facturas.Remove(facturas);
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