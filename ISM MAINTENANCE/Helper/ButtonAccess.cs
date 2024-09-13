using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class ButtonAccess
    {   
        public bool GetAccess(List<string> obj, string button) {
            bool result = obj.Any(y => y.Contains(button));
            return result;      
        }
    }
}
