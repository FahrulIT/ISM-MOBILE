using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_MAINTENANCE.Models.ViewModel.Report
{
    public class FormParameterPrev
    {
        public decimal dept_par { get; set; }
        public decimal mc_id_par { get; set; }
        //public DateTime start_date_par { get; set; }
        //public DateTime stop_date_par { get; set; }
        public string start_date_par { get; set; }
        public string stop_date_par { get; set; }
        public string machine_par { get; set; }
        public string PIC_Mtc { get; set; }

        public List<PreventiveMaintenance> Report_Data { get; set; }

    }

    public class PreventiveMaintenance
    {
        public decimal dept_id { get; set; }

        public string dept_name { get; set; }
        public decimal mc_id { get; set; }
        public string mc_name { get; set; }
        public DateTime mtc_schedule { get; set; }

        public DateTime? mtc_schedule_rpt { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public DateTime? mtc_start { get; set; }
        public DateTime? mtc_finish { get; set; }
        public string Pic_Maintenance { get; set; }
        public string Pic_Maintenance_Name { get; set; }
        public string mtc_id { get; set; }
        public string mtc_name { get; set; }
        public string mtc_action_id { get; set; }
        public string mtc_action_name { get; set; }
        public string item_code { get; set; }
        public decimal? price { get; set; }
        public decimal? quantity { get; set; }
        //public DateTime? Duration { get; set; }
        public string nama_sparepart { get; set; }
		public string no_catalog { get; set; }
		public int RowNumber { get; set; }
        
        public string Duration
        {
            get
            {
                if (mtc_finish == null)
                {
                    return null;
                }
                else
                {
                    string result;
                    var selisih = mtc_finish.Value - mtc_start;
                    if (selisih != null)
                    {
                        result = selisih.Value.Days.ToString();

                        //if (result == "0")
                        //{
                        //    result = (selisih.Hours.ToString().Length < 2 ? "0" + selisih.Hours.ToString() : selisih.Hours.ToString()) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
                        //}
                        //else
                        //{
                        //    result = result + ":" + (selisih.Hours.ToString().Length < 2 ? "0" + selisih.Hours.ToString() : selisih.Hours.ToString()) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
                        //}


                        if (result == "0")
                        {
                            result = (selisih.Value.Hours.ToString().Length < 2 ? "0" + selisih.Value.Hours.ToString() : selisih.Value.Hours.ToString()) + ":" + (selisih.Value.Minutes.ToString().Length < 2 ? "0" + selisih.Value.Minutes.ToString() : selisih.Value.Minutes.ToString());
                        }
                        else
                        {
                            result = Convert.ToString(selisih.Value.Hours + (Convert.ToInt16(result) * 24)) + ":" + (selisih.Value.Minutes.ToString().Length < 2 ? "0" + selisih.Value.Minutes.ToString() : selisih.Value.Minutes.ToString());
                        }

                    }
                    else
                    {
                        result = "";
                    }

                    return result;
                }

            }
        }
    }
}