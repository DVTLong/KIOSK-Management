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
    
    public partial class HinhThuc_ThanhToan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HinhThuc_ThanhToan()
        {
            this.HoaDons = new HashSet<HoaDon>();
        }
    
        public int MaHTTT { get; set; }
        public string TenHTTT { get; set; }
        public Nullable<bool> TrangThaiPV { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDons { get; set; }
    }
}
