//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KIOSK_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LichSu_DangNhap
    {
        public int ID { get; set; }
        public System.DateTime NgayDangNhap { get; set; }
        public System.TimeSpan ThoiGian { get; set; }
        public string ViTri { get; set; }
        public int MaTK { get; set; }
        public Nullable<bool> TrangThaiPV { get; set; }
    
        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
