using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KIOSK_Management.Models;
using PagedList;

namespace KIOSK_Management.Controllers
{
    public class TaiKhoanAdminsController : Controller
    {
        private QLKIOSKEntities db = new QLKIOSKEntities();

        // GET: TaiKhoans
        public ActionResult Index(int? page)
        {
            Config cf_pagesize = db.Configs.SingleOrDefault(x => x.variable_name.Equals("taikhoanadmins_index_pagesize"));
            int pagenum = page ?? 1;
            int pagesize = cf_pagesize == null ? 5 : Convert.ToInt32(cf_pagesize.value);
            var taiKhoans = db.V_TaiKhoanAdmin.ToList();
            return View(taiKhoans.ToPagedList(pagenum, pagesize));
        }

        public ActionResult _GetAccountsByLoaiNhom(int maln, int? page)
        {
            Config cf_pagesize = db.Configs.SingleOrDefault(x => x.variable_name.Equals("taikhoanadmins_index_pagesize"));
            int pagenum = page ?? 1;
            int pagesize = cf_pagesize == null ? 5 : Convert.ToInt32(cf_pagesize.value);

            
            List<sp_GetAccountsByMaLN_Result> taiKhoans = db.sp_GetAccountsByMaLN(maln).ToList();
            return PartialView(taiKhoans.ToPagedList(pagenum, pagesize));
        }

        // GET: TaiKhoans/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection f)
        {
            string cmnd = f["CMND"];
            string tenkh = f["TenKH"];
            string diachi = f["DiaChi"];
            string email = f["Email"];
            string sdt = f["SDT"];

            int loainhom = Convert.ToInt32(f["LoaiNhom"]);
            string username = f["Username"];
            string password = f["Password"];
            string repassword = f["RetypePassword"];
            bool trangthai = Convert.ToBoolean(f["TrangThai"]);

            if (Utility.StringIsInvalid(username) || username.Length > 50)
            {
                ViewBag.Validate_Username = "Invalid username";
                return View();
            }

            if (Utility.StringIsInvalid(password))
            {
                ViewBag.Validate_Password = "Invalid password";
                return View();
            }

            if (!password.Equals(repassword))
            {
                ViewBag.Validate_MatchPassword = "Password does not match";
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    password = Utility.CreateMD5(Utility.Base64Encode(password));
                    db.sp_KhachHang_INSERT(cmnd, tenkh, diachi, email, sdt);
                    int makh = db.MaKHMoi().First().Value;
                    db.sp_TaiKhoan_INSERT(username, password, trangthai, makh, null);
                    int matk = db.TKMoi().First().Value;
                    List<int?> macns = db.sp_ChucNangTheoLoaiNhom(loainhom).ToList();
                    if (macns.Count > 0)
                    {
                        foreach (int macn in macns)
                        {
                            db.sp_PhanQuyen_INSERT(matk, macn, false);
                        }
                        
                    }

                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
                }
            }

            
            return View();
        }

        public JsonResult CheckKhachHang(string cmnd)
        {
            int count = db.checkKH(cmnd).ToList().Count;
            if (count == 0)
                return Json(count, JsonRequestBehavior.AllowGet);
            else
            {
                var query = db.checkKH(cmnd).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }

        [ChildActionOnly]
        public ActionResult _DropDown_LoaiNhom()
        {
            List<LoaiNhom> list = db.LoaiNhoms.Where(x=>x.MaLN != 1 && x.MaLN != 4).ToList();
            return PartialView(list);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Enable(int matk)
        {
            try
            {
                TaiKhoan tk = db.TaiKhoans.SingleOrDefault(x => x.MaTK == matk);
                if (tk == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                db.sp_TaiKhoan_Enable(tk.MaTK);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        public ActionResult Disable(int matk)
        {
            try
            {
                TaiKhoan tk = db.TaiKhoans.SingleOrDefault(x => x.MaTK == matk);
                if (tk == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }

                db.sp_TaiKhoan_Disable(tk.MaTK);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }




        //end class
    }
}
