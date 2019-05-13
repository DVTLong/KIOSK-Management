using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;

namespace KIOSK_Management.Controllers
{
    public class PhanQuyenAdminsController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: PhanQuyenAdmin
        public ActionResult Edit(int matk)
        {
            List<PhanQuyen> list = db.PhanQuyens.Include("ChucNang").Where(x => x.MaTK == matk).ToList();
            ViewBag.MaTK = matk;
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection f)
        {
            int matk = Convert.ToInt32(f["MaTK"]);
            List<PhanQuyen> list = db.PhanQuyens.Where(x => x.MaTK == matk).ToList();
            foreach (PhanQuyen q in list)
            {
                bool tinhtrang = Convert.ToBoolean(f[q.MaCN.ToString()]);
                try
                {
                    db.sp_PhanQuyen_UPDATE(matk, q.MaCN, tinhtrang);
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }
            return RedirectToAction("Index", "TaiKhoanAdmins");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}