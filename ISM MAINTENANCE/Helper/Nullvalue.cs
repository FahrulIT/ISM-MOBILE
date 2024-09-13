using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Nullvalue
    {
        public string ReplaceNullValueString(object obj)
        {
            string _value = "";

            if (obj == null)
            {
                _value = "";
            }
            else
            {
                _value = obj.ToString();
            }

            return _value;
        }

        public decimal ReplaceNullValueNumeric(object obj)
        {
            decimal _value = 0;
            if (obj == null)
            {
                _value = 0;
            }
            else
            {
                _value = Convert.ToDecimal(obj);
            }

            return _value;
        }

    }



}
