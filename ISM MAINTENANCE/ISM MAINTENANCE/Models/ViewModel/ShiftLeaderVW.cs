using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class ShiftLeaderNotes

    {

        [Key]
        [Required]
        public decimal dept_id { get; set; }

        [Key]
        [Required]
        public decimal mc_id { get; set; }

        [Key]
        [Required(ErrorMessage = "Tanggal Note belum diinput !!!")]
        [Display(Name = "Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }

        [Key]
        [Required(ErrorMessage = "Shift belum Dipilih !!!")]
        [Display(Name = "Shift")]
        public string shift { get; set; }

        [Key]
        public string block { get; set; }

        [Key]
        public string loom_no { get; set; }

        [Required]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Machine Number belum dipilih !!!")]
        [Display(Name = "Machine No")]
        public string machine_no { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        [Display(Name = "Machine")]
        public string mc_name { get; set; }


        [Display(Name = "Stop From")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime stop_from { get; set; }

        [Display(Name = "Stop To")]
        public System.DateTime? stop_to { get; set; }

        [Required(ErrorMessage = "Nama Pimpinan belum dipilih !!!")]
        [Display(Name = "Shift Leader Name")]
        public string pic { get; set; }

        [Display(Name = "PIC")]
        public string PicName { get; set; }

        public string DateInput { get; set; }

        [Required(ErrorMessage = "Tanggal stop mesin belum diinput !!!")]
        [Display(Name = "Stop From")]
        public string stop_from_str
        {
            get; set;
        }

        [Required(ErrorMessage = "Nama trouble belum diinput !!!")]
        public List<ShiftLeaderNotesDetail> ShiftLeaderNotesDetails { get; set; }

    }

    public class ShiftLeaderNotesDetail
    {

        public decimal? dept_id { get; set; }

        public decimal? mc_id { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }

        public string shift { get; set; }

        public string block { get; set; }

        public string loom_no { get; set; }

        [Key]
        [Required(ErrorMessage = "Nama trouble belum diinput !!!")]
        public string mtc_id { get; set; }

        [Display(Name = "MTC Type")]
        public string mtc_name { get; set; }

        [Key]
        [Required(ErrorMessage = "Nama trouble belum diinput !!!")]
        public string trouble_id { get; set; }

        [Display(Name = "Trouble Name")]
        public string trouble_name { get; set; }

        //[Display(Name = "Machine No")]
        //public string machine_no { get; set; }

    }

    public class ShiftLeaderNotesView
    {



        public decimal dept_id { get; set; }

        [Display(Name = "Department")]
        public string dept_name { get; set; }

        public decimal mc_id { get; set; }

        [Display(Name = "Machine")]
        public string mc_name { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime note_date { get; set; }

        [Display(Name = "Shift")]
        public string shift { get; set; }
        public string block { get; set; }
        public string loom_no { get; set; }

        [Display(Name = "M/C No")]
        public string machine_no { get; set; }

        [Display(Name = "Shift Leader Name")]
        public string pic { get; set; }

        [Display(Name = "Shift Leader")]
        public string PicName { get; set; }

        [Display(Name = "Stop From")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public System.DateTime stop_from { get; set; }

        [Display(Name = "Stop To")]
        public System.DateTime? stop_to { get; set; }

        [Display(Name = "MTC Type")]
        public string mtc_name { get; set; }

        [Display(Name = "Lama Stop")]
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
}