//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISM_MAINTENANCE.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_preventivemaintenance_sparepart
    {
        public string ItemName { get; set; }
        public decimal price { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public System.DateTime mtc_schedule { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public string item_code { get; set; }
        public string dept_name { get; set; }
        public string mc_name { get; set; }
    }
}