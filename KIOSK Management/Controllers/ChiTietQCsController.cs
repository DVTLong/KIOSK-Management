using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;
using System.Data.SqlClient;
using PagedList;

namespace KIOSK_Management.Controllers
{
    public class ChiTietQCsController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: ChiTietQCs
        public ActionResult Index(int? page)
        {
            Config cf_pagesize = db.Configs.SingleOrDefault(x => x.variable_name.Equals("chitietqcs_index_pagesize"));
            int pagenum = page ?? 1;
            int pagesize = cf_pagesize == null ? 5 : Convert.ToInt32(cf_pagesize.value);

            List<V_ChiTietQC> list = db.V_ChiTietQC.ToList();
            return View(list.OrderByDescending(x => x.MaQC).ToPagedList(pagenum, pagesize));
        }

        // GET: ChiTietQCs/Create
        public ActionResult Create(int sohd, string mako)
        {
            ChiTietThueKIOSKQC chiTietThue = db.ChiTietThueKIOSKQCs.SingleOrDefault(x => x.SoHD == sohd && x.MAKO.Equals(mako));
            if (chiTietThue == null)
            {
                return RedirectToAction("Index", "Chitietthue");
            }

            return View();
        }

        // POST: ChiTietQCs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            int sohd = Convert.ToInt32(f["SoHD"]);
            string mako = f["MAKO"];
            int qtyTotal = Convert.ToInt32(f["qtyTotal"]);

            List<ChiTietQC> chiTietQCs = new List<ChiTietQC>();
            for (int i=0; i < qtyTotal; i++)
            {
                int maqc = Convert.ToInt32(f["maqc_" + i]);
                DateTime ngaybd = DateTime.Parse(f["ngaybdqc_" + i]);
                DateTime ngaykt = DateTime.Parse(f["ngayktqc_" + i]);

                if (!(ngaybd <= ngaykt))
                {
                    ViewBag.Validate_ngay = "NgayBD or NgayKT invalid";
                    return View();
                }

                ChiTietQC chiTietQC = new ChiTietQC()
                {
                    SoHD = sohd,
                    MAKO = mako,
                    MaQC = maqc,
                    NgayBDQC = ngaybd,
                    NgayKTQC = ngaykt
                };
                chiTietQCs.Add(chiTietQC);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (ChiTietQC ct in chiTietQCs)
                    {
                        db.sp_ChiTietQC_INSERT(ct.SoHD, ct.MAKO, ct.MaQC, ct.NgayBDQC, ct.NgayKTQC);
                    }
                    
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            return RedirectToAction("Index");
        }


        // GET: ChiTietQCs/Delete/5
        public ActionResult Delete(int SoHD, string MaKO, int MaQC)
        {
            ChiTietQC chiTietQC = db.ChiTietQCs.SingleOrDefault(x => x.SoHD == SoHD && x.MAKO.Equals(MaKO) && x.MaQC == MaQC);
            if (chiTietQC == null)
            {
                return HttpNotFound();
            }
            return View(chiTietQC);
        }

        // POST: ChiTietQCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int SoHD, string MaKO, int MaQC)
        {
            try
            {
                db.sp_ChiTietQC_DELETE(SoHD, MaKO, MaQC);
            }
            catch (SqlException ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult _DropDown_KIOSK(int sohd)
        {
            List<ChiTietThueKIOSKQC> list = db.ChiTietThueKIOSKQCs.Include(x=>x.KIOSK).Where(x=>x.SoHD == sohd).ToList();
            return PartialView(list);
        }

        public ActionResult _QuangCaoRow()
        {
            List<QuangCao> list = db.QuangCaos.ToList();
            return PartialView(list);
        }

    }
}
