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
    
    public partial class tr_shift_leader_note
    {
        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public System.DateTime note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public System.DateTime stop_from { get; set; }
        public Nullable<System.DateTime> stop_to { get; set; }
        public string pic { get; set; }
        public string user_id { get; set; }
        public string rec_sts { get; set; }
        public Nullable<int> proc_no { get; set; }
        public string client_ip { get; set; }
        public Nullable<System.DateTime> proc_time { get; set; }
    }
}
