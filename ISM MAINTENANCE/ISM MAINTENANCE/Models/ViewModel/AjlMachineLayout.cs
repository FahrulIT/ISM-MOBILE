using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISM_MAINTENANCE.Models.ViewModel
{
    public class BlockA
    {
        public string MC { get; set; }
        public string Color { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }
    }

    public class BlockB
    {
        public string MC { get; set; }
        public string Color { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }
    }

    public class BlockC
    {
        public string MC { get; set; }
        public string Color { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }
    }

    public class BlockD
    {
        public string MC { get; set; }
        public string Color { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }
    }

    public class BlockAll
    {
        public string MC { get; set; }
        public string Color { get; set; }
        public int? RowPos { get; set; }
        public int? ColPos { get; set; }
    }

    public class TroubleCount
        {
        public string mtc_name { get; set; }
        public string block { get; set; }
        public int JumlahTrouble { get; set; }

    }
}