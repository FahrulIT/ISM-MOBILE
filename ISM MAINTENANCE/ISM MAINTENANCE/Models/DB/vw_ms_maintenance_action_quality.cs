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
    
    public partial class vw_ms_maintenance_action_quality
    {
        public string department { get; set; }
        public string machine { get; set; }
        public string troubelName { get; set; }
        public string mtcActionName { get; set; }
        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public string trouble_id { get; set; }
        public string mtc_action_id { get; set; }
        public string mtc_id { get; set; }
        public string mtc_action_name { get; set; }
        public int transactionUse { get; set; }
    }
}