
namespace ISM_MOBILE_APPLICATION.Models.Home
{
    public class DetailHelper
    {
        public long id { get; set; }

        public int Bulan { get; set; }

        public int Tahun { get; set; }

        public string Dept { get; set; }

        public string Item { get; set; }

        public string Data_type { get; set; }

        //Handling Null Value
        //from model
        private decimal? _Persentase;
        public decimal? Persentase
        {
            get
            {
                return _Persentase;
            }
            set
            {
                _Persentase = value ?? 0;
            }
        }

        private decimal? _Plan;
        public decimal? Plan
        {
            get
            {
                return _Plan;
            }
            set
            {
                _Plan = value ?? 0;
            }
        }


        private decimal? _Actual;
        public decimal? Actual
        {
            get
            {
                return _Actual;
            }
            set
            {
                _Actual = value ?? 0;
            }
        }

        private decimal? _PlanofMonth;
        public decimal? PlanofMonth
        {
            get
            {
                return _PlanofMonth;
            }
            set
            {
                _PlanofMonth = value ?? 0;
            }
        }

        private decimal? _CurrentInv;
        public decimal? CurrentInv
        {
            get
            {
                return _CurrentInv ; }
            set
            {
                _CurrentInv = value ?? 0;
            }
        }

    }
}