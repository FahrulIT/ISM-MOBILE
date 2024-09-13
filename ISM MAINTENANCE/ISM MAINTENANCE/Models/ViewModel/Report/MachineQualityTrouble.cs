using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel.Report
{

    public class FormParameter
    {
        public decimal dept_par { get; set; }
        public decimal mc_id_par { get; set; }
        public DateTime start_date_par { get; set; }
        public DateTime stop_date_par { get; set; }
        public string trouble_type_par { get; set; }
        public string machine_par { get; set; }
        public string PIC_Mtc { get; set; }
        public string PIC_Leader { get; set; }

        public string start_date{ get; set; }
        public string stop_date { get; set; }

        public List<MachineQualityTrouble> Report_Data { get; set; }

    }

    public class MachineQualityTrouble
    {
        public int? No { get; set; }
        public int? RowNo { get; set; }
        public decimal? dept_id { get; set; }
        public string dept_name { get; set; }
        public decimal? mc_id { get; set; }
        public string mc_name { get; set; }
        public DateTime? note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public string Pic_LeaderShift { get; set; }
        public string Pic_LeaderShift_Name { get; set; }
        public DateTime? stop_from { get; set; }
        public DateTime? stop_to { get; set; }
        public string trouble_id { get; set; }
        public string trouble_name { get; set; }
        public DateTime? mtc_start { get; set; }
        public DateTime? mtc_finish { get; set; }
        public string Pic_Maintenance { get; set; }
        public string Pic_Maintenance_Name { get; set; }
        public string mtc_id { get; set; }
        public string mtc_name { get; set; }
        public string mtc_action_id { get; set; }
        public string mtc_action_name { get; set; }
        public string item_code { get; set; }

        //[Display(Name ="Nama Sparepart")]
        public string sparepart_name { get; set; }

        //[Display(Name ="Nomor Catalogue")]
        public string no_catalog { get; set; }

        public decimal? price { get; set; }
        public decimal? quantity { get; set; }
        //public DateTime? Duration { get; set; }
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

   

    public class MAXROW_MAINTENANCE
    {

        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public DateTime note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }                 
        public string mtc_id { get; set; }   
        public System.Int64 Maxrow { get; set; }

    }


    public class MAINTENANCE_TROUBLE
    {

        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public DateTime note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public DateTime? mtc_start { get; set; }
        public DateTime? mtc_finish { get; set; }
        public string finish_sts { get; set; }
        public string mtc_id { get; set; }
        public string trouble_id { get; set; }   

        public string pic_mtc { get; set; }
        public System.Int64 row { get; set; }

    }

    public class MAINTENANCE_ACTION
    {

        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public DateTime note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public DateTime? mtc_start { get; set; }
        public DateTime? mtc_finish { get; set; }
        public string finish_sts { get; set; }
        public string mtc_id { get; set; }
        public string trouble_id { get; set; }
        public string mtc_action_id { get; set; }

        public string pic_mtc { get; set; }
        public System.Int64 row { get; set; }

    }


    public class SPAREPART
    {
        public string ITEM_CODE { get; set; }


        public string sparepart_name { get; set; }


        public string no_catalog { get; set; }

        public decimal dept_id { get; set; }
        public decimal mc_id { get; set; }
        public DateTime note_date { get; set; }
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }
        public decimal? price { get; set; }
        public decimal? quantity { get; set;}

        public System.Int64 row { get; set; }
    }

}