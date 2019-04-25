using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;
using PagedList;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

namespace KIOSK_Management.Controllers
{
    public class KIOSKsController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: KIOSKs
        public ActionResult Index(int? page)
        {
            Config cf_pagesize = db.Configs.SingleOrDefault(x => x.variable_name.Equals("kiosks_index_pagesize"));
            int pagenum = page ?? 1;
            int pagesize = cf_pagesize == null ? 5 : Convert.ToInt32(cf_pagesize.value);
            List<V_KIOSK> list = db.V_KIOSK.ToList();
            return View(list.OrderBy(x=>x.NgayXD).ToPagedList(pagenum, pagesize));
        }

        // GET: KIOSKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KIOSKs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAKO,TenKO,NgayXD,NgayVH,DiaDiem,TrangThaiKo,ImageBanner,ConnectStr")] KIOSK kIOSK, HttpPostedFileBase fImageBanner)
        {
            //Validate
            string mako = kIOSK.MAKO.Trim();
            string tenko = kIOSK.TenKO == null ? kIOSK.TenKO : kIOSK.TenKO.Trim();
            string diadiem = kIOSK.DiaDiem == null ? kIOSK.DiaDiem : kIOSK.DiaDiem.Trim();
            string connectstr = kIOSK.ConnectStr == null ? kIOSK.ConnectStr : kIOSK.ConnectStr.Trim();

            if (Utility.StringIsInvalid(mako) || mako.Length > 18)
            {
                ViewBag.validate_mako = "MaKO is invalid";
                return View(kIOSK);
            }
            if (tenko != null && (Utility.StringIsInvalid(tenko) || tenko.Length > 100))
            {
                ViewBag.validate_tenko = "TenKO is invalid";
                return View(kIOSK);
            }
            if (diadiem != null && (Utility.StringIsInvalid(diadiem) || diadiem.Length > 100))
            {
                ViewBag.validate_diadiem = "DiaDiem is invalid";
                return View(kIOSK);
            }
            if (connectstr != null && (Utility.StringIsInvalid(connectstr) || connectstr.Length > 200))
            {
                ViewBag.validate_connectstr = "ConnectStr is invalid";
                return View(kIOSK);
            }


            string filename = string.Empty;
            if (fImageBanner != null)
            {
                string extension = fImageBanner.ContentType.Split('/')[1];
                filename = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string targetFolder = Server.MapPath("~/Images/KIOSK");
                string targetPath = Path.Combine(targetFolder, filename);
                fImageBanner.SaveAs(targetPath);
                //Save file if not exists
                if (!System.IO.File.Exists(targetPath))
                {
                    fImageBanner.SaveAs(targetPath);
                }
            }
            else
            {
                filename = kIOSK.ImageBanner;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_KIOSK_INSERT(mako, tenko, kIOSK.NgayXD, kIOSK.NgayVH, diadiem, "0", kIOSK.TrangThaiKo, filename, connectstr);
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            return View(kIOSK);
        }

        // GET: KIOSKs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KIOSK kIOSK = db.KIOSKs.Find(id);
            if (kIOSK == null)
            {
                return HttpNotFound();
            }
            return View(kIOSK);
        }

        // POST: KIOSKs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAKO,TenKO,NgayXD,NgayVH,DiaDiem,TrangThaiKo,ImageBanner,ConnectStr")] KIOSK kIOSK, HttpPostedFileBase fImageBanner)
        {
            //Validate
            string mako = kIOSK.MAKO.Trim();
            string tenko = kIOSK.TenKO == null ? kIOSK.TenKO : kIOSK.TenKO.Trim();
            string diadiem = kIOSK.DiaDiem == null ? kIOSK.DiaDiem : kIOSK.DiaDiem.Trim();
            string connectstr = kIOSK.ConnectStr == null ? kIOSK.ConnectStr : kIOSK.ConnectStr.Trim();

            if (Utility.StringIsInvalid(mako) || mako.Length > 18)
            {
                ViewBag.validate_mako = "MaKO is invalid";
                return View(kIOSK);
            }
            if (tenko != null && (Utility.StringIsInvalid(tenko) || tenko.Length > 100))
            {
                ViewBag.validate_tenko = "TenKO is invalid";
                return View(kIOSK);
            }
            if (diadiem != null && (Utility.StringIsInvalid(diadiem) || diadiem.Length > 100))
            {
                ViewBag.validate_diadiem = "DiaDiem is invalid";
                return View(kIOSK);
            }
            if (connectstr != null && (Utility.StringIsInvalid(connectstr) || connectstr.Length > 200))
            {
                ViewBag.validate_connectstr = "ConnectStr is invalid";
                return View(kIOSK);
            }

            string filename = string.Empty;
            if (fImageBanner != null)
            {
                string extension = fImageBanner.ContentType.Split('/')[1];
                filename = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                string targetFolder = Server.MapPath("~/Images/KIOSK");
                string targetPath = Path.Combine(targetFolder, filename);
                fImageBanner.SaveAs(targetPath);
                //Save file if not exists
                if (!System.IO.File.Exists(targetPath))
                {
                    fImageBanner.SaveAs(targetPath);
                }
            }
            else
            {
                filename = kIOSK.ImageBanner;
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_KIOSK_UPDATE(mako, tenko, kIOSK.NgayXD, kIOSK.NgayVH, diadiem, "0", kIOSK.TrangThaiKo, filename, connectstr);
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            return View(kIOSK);
        }

        // GET: KIOSKs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KIOSK kIOSK = db.KIOSKs.Find(id);
            if (kIOSK == null)
            {
                return HttpNotFound();
            }
            return View(kIOSK);
        }

        // POST: KIOSKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KIOSK kIOSK = db.KIOSKs.Find(id);
            if (kIOSK == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.sp_KIOSK_DELETE(id);
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
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

        [ChildActionOnly]
        public ActionResult _ClearToken()
        {
            string password = ConfigurationManager.AppSettings["apipass"].ToString();
            ViewBag.APIPassword = password;
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> ClearToken(FormCollection f)
        {
            string password = f["password"];
            bool response = await KIOSKAPI_Web.ClearAllTokensAsync(password);
            return View(response);
        }
        
    }
}
