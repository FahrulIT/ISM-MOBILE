using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class MaintenanceHistory
    {
        [Key]
        public decimal dept_id { get; set; }
        [Key]
        public decimal mc_id { get; set; }
        [Key]
        public System.DateTime note_date { get; set; }
        [Key]
        [Display(Name = "Shift")]
        public string shift { get; set; }
        [Key]
        public string block { get; set; }
        [Key]
        public string loom_no { get; set; }

        [Display(Name = "M/C No")]
        public string machine_no { get; set; }
        public string dept_name { get; set; }

        [Display(Name = "Start MTC")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime mtc_start { get; set; }

        [Display(Name = "Finish MTC")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? mtc_finish { get; set; }

        [Display(Name = "Duration")]
        public string CalculateTime
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
                        result = selisih.Days.ToString();

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
                            result = (selisih.Hours.ToString().Length < 2 ? "0" + selisih.Hours.ToString() : selisih.Hours.ToString()) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
                        }
                        else
                        {
                            result = Convert.ToString(selisih.Hours + (Convert.ToInt16(result) * 24)) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
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

        public string pic_mtc { get; set; }

        [Display(Name = "PIC MTC")]
        public string pic_mtc_name { get; set; }

        [Display(Name = "Status")]
        public string finish_sts { get; set; }

        [Display(Name = "MTC Type")]
        public string mtc_name { get; set; }

    }

    public class MaintenanceHistoryHeader
    {
        [Key]
        public decimal dept_id { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public string block { get; set; }
        [Key]
        public string loom { get; set; }

        [Key]
        [Display(Name = "Date of Shift Leader Notes")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }

        [Key]
        [Required(ErrorMessage = "Shift masih kosong, choose data pimpinan shift dahulu")]
        [Display(Name = "Shift")]
        public string shift { get; set; }

        [Required(ErrorMessage = "Machine Number masih kosong, choose data pimpinan shift dahulu !!!")]
        [Display(Name = "Machine No")]
        public string machine_no { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        [Display(Name = "Machine Type")]
        public string mc_type { get; set; }

        [Display(Name = "M/C Stop From")]

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss}")]
        public DateTime? stop_from { get; set; }

        [Required(ErrorMessage = "Tanggal mesin stop masih kosong, choose data pimpinan shift dahulu")]
        public string stop_from_str { get; set; }

        [Required(ErrorMessage = "Tanggal note date masih kosong, choose data pimpinan shift dahulu")]
        public string TanggalString { get; set; }

        [Required(ErrorMessage = "PIC Maintenance belum dipilih !!!")]
        public string pic_id { get; set; }

        [Display(Name = "PIC Maintenance")]
        public string pic_name { get; set; }

        [Display(Name = "Start Maintenance")]
        public System.DateTime mtc_start { get; set; }

        [Display(Name = "Finish Maintenance")]
        public DateTime? mtc_stop { get; set; }

        [Display(Name = "Maintenance Status")]
        public string finish_sts { get; set; }

        [Required(ErrorMessage = "Waktu maintenance start belum dipilih !!!")]
        public string mtc_start_str { get; set; }

        public string mtc_stop_str { get; set; }

        [Required(ErrorMessage = "Maintenance Action belum diinput !!!")]
        public List<MtcAction_Detail> Detail_1 { get; set; }
              
        public List<MtcSparepart_Detail> Detail_2 { get; set; }

    }

    public class MtcAction_Detail
    {
        [Key]
        public string mtc_id { get; set; }
        [Key]
        public string trouble_id { get; set; }

        [Key]
        public string mtc_action_id { get; set; }

        [Display(Name = "Maintenance Action")]
        public string mtc_action_name { get; set; }

        [Display(Name = "Trouble Name")]
        public string trouble_name { get; set; }

        [Display(Name = "MTC Type")]
        public string mtc_type { get; set; }

    }

    public class MtcAction_Shift
    {
        [Key]
        public string mtc_id { get; set; }
        [Key]
        public string trouble_id { get; set; }

        [Key]
        public string mtc_action_id { get; set; }

        [Display(Name = "Maintenance Action")]
        public string mtc_action_name { get; set; }

        [Display(Name = "Trouble Name")]
        public string trouble_name { get; set; }

        [Display(Name = "MTC Type")]
        public string mtc_type { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }

        [Display(Name = "Shift")]
        public string shift { get; set; }
        public string block { get; set; }
        public string loom { get; set; }

    }

    public class MtcSparepart_Detail
    {
        [Key]
        public string item_code { get; set; }

        [Display(Name = "Spare Part")]
        public string item_name { get; set; }

        [Display(Name = "Unit Price")]
        //[RegularExpression(@"^\d+(\.\d+)+$", ErrorMessage = "Input Price harus angka disertai titik untuk decimal Contoh : 1.00")]
        //[DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal price { get; set; }

        [Display(Name = "Qty")]
        //[RegularExpression(@"^\d+(\.\d+)+$", ErrorMessage = "Input Qty harus angka disertai titik untuk decimal Contoh : 100.00")]
        //[DisplayFormat(DataFormatString = "{0:0.##}", ApplyFormatInEditMode = true)]
        public decimal quantity { get; set; }
    }

    public class LookupShiftLeaderNotes
    {
        [Key]
        public string block { get; set; }
        [Key]
        public string loom_no { get; set; }
        [Key]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }
        [Key]
        public System.DateTime stop_from { get; set; }
        [Key]
        [Display(Name = "Shift")]    
        public string shift { get; set; }

        [Key]
        public decimal mc_id { get; set; }

        [Key]
        public decimal dept_id { get; set; }


        public string machine_no { get; set; }
     
        public string LamaStop
        {
            get
            {
                if (stop_from == null)
                {
                    return null;
                }
                else
                {
                    string result;
                    DateTime Today = DateTime.Now;

                    var selisih = Today - stop_from;
                    if (selisih != null)
                    {
                        result = selisih.Days.ToString();

                        if (result == "0")
                        {
                            result = (selisih.Hours.ToString().Length < 2 ? "0" + selisih.Hours.ToString() : selisih.Hours.ToString()) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
                        }
                        else
                        {
                            result = Convert.ToString(selisih.Hours + (Convert.ToInt16(result) * 24)) + ":" + (selisih.Minutes.ToString().Length < 2 ? "0" + selisih.Minutes.ToString() : selisih.Minutes.ToString());
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
        public double? LamaStopNumerik
        {
            get
            {
                if (stop_from == null)
                {
                    return null;
                }
                else
                {
                    DateTime Today = DateTime.Now;

                    var selisih = Today - stop_from;
                    if (selisih != null)
                    {
                        return selisih.TotalHours;

                    }
                    else
                    {
                        return 0;
                    }
                }

            }
        }
    }

    public class SAPITEM
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public decimal Price { get; set; }
    }
}