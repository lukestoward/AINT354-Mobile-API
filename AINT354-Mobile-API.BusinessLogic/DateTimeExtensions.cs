using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.BusinessLogic
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime src)
        {
            return src.ToShortDateString() + " " + src.ToShortTimeString();
        }
    }
}
