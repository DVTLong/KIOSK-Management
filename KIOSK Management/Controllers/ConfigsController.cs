using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;

namespace KIOSK_Management.Controllers
{
    public class ConfigsController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: Configs
        public ActionResult Index()
        {
            return View(db.Configs.ToList());
        }

        // GET: Configs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Config config = db.Configs.Find(id);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

        // POST: Configs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "variable_name,value")] Config config)
        {
            if (config.variable_name.Equals("kiosks_index_pagesize"))
            {
                if (!IsNumber(config.value))
                {
                    ViewBag.Validate_kiosks_index_pagesize = "Pagesize is invalid";
                    return View(config);
                }
            }

            if (ModelState.IsValid)
            {
                db.sp_CONFIG_UPDATE(config.variable_name, config.value);
                return RedirectToAction("Index");
            }
            return View(config);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsNumber(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
