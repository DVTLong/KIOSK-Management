using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;
using PagedList;

namespace KIOSK_Management.Controllers
{
    public class QuangCaosController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: QuangCaos
        public ActionResult Index(int? page)
        {
            Config cf_pagesize = db.Configs.SingleOrDefault(x => x.variable_name.Equals("quangcaos_index_pagesize"));
            int pagenum = page ?? 1;
            int pagesize = cf_pagesize == null ? 5 : Convert.ToInt32(cf_pagesize.value);

            List<V_QuangCao> list = db.V_QuangCao.ToList();
            return View(list.OrderByDescending(x => x.MaQC).ToPagedList(pagenum, pagesize));
        }

        // GET: QuangCaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuangCaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaQC,NoiDung,HinhAnh,ThoiLuong")] QuangCao quangCao, HttpPostedFileBase HinhAnh)
        {
            string noidung = quangCao.NoiDung == null ? quangCao.NoiDung : quangCao.NoiDung.Trim();
            int thoiluong = quangCao.ThoiLuong;

            if (noidung != null && (Utility.StringIsInvalid(noidung) || noidung.Length > 200))
            {
                ViewBag.Validate_NoiDung = "";
                return View(quangCao);
            }

            if (thoiluong < 0)
            {
                ViewBag.Validate_ThoiLuong = "";
                return View(quangCao);
            }

            string filename = string.Empty;
            if (HinhAnh != null)
            {
                //Create folder if not exists
                //Directory.CreateDirectory("~/Images");
                string extension = HinhAnh.ContentType.Split('/')[1];
                filename = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string targetFolder = Server.MapPath("~/Images/QuangCao");
                string targetPath = Path.Combine(targetFolder, filename);
                HinhAnh.SaveAs(targetPath);
                //Save file if not exists
                if (!System.IO.File.Exists(targetPath))
                {
                    HinhAnh.SaveAs(targetPath);
                }
            }
            else
            {
                filename = quangCao.HinhAnh;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_QuangCao_INSERT(noidung, filename, thoiluong);
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            return View(quangCao);
        }

        // GET: QuangCaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuangCao quangCao = db.QuangCaos.Find(id);
            if (quangCao == null)
            {
                return HttpNotFound();
            }
            return View(quangCao);
        }

        // POST: QuangCaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaQC,NoiDung,HinhAnh,ThoiLuong")] QuangCao quangCao, HttpPostedFileBase HinhAnh)
        {
            string noidung = quangCao.NoiDung == null ? quangCao.NoiDung : quangCao.NoiDung.Trim();
            int thoiluong = quangCao.ThoiLuong;

            if (noidung != null && (Utility.StringIsInvalid(noidung) || noidung.Length > 200))
            {
                ViewBag.Validate_NoiDung = "";
                return View(quangCao);
            }

            if (thoiluong < 0)
            {
                ViewBag.Validate_ThoiLuong = "";
                return View(quangCao);
            }

            string filename = string.Empty;
            if (HinhAnh != null)
            {
                //Create folder if not exists
                //Directory.CreateDirectory("~/Images");
                string extension = HinhAnh.ContentType.Split('/')[1];
                filename = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string targetFolder = Server.MapPath("~/Images/QuangCao");
                string targetPath = Path.Combine(targetFolder, filename);
                HinhAnh.SaveAs(targetPath);
                //Save file if not exists
                if (!System.IO.File.Exists(targetPath))
                {
                    HinhAnh.SaveAs(targetPath);
                }
            }
            else
            {
                filename = quangCao.HinhAnh;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_QuangCao_UPDATE(quangCao.MaQC, noidung, filename, thoiluong);
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            return View(quangCao);
        }

        // GET: QuangCaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuangCao quangCao = db.QuangCaos.Find(id);
            if (quangCao == null)
            {
                return HttpNotFound();
            }
            return View(quangCao);
        }

        // POST: QuangCaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.sp_QuangCao_DELETE(id);
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
    }
}
