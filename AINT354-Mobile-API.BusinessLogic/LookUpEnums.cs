using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.BusinessLogic
{
    public static class LookUpEnums
    {
        public enum Colours
        {
            Red = 1,
            Green = 2,
            Blue = 3,
            Orange = 4
        }

        public enum CalendarTypes
        {
            Public = 1,
            Private = 2
        }

        public enum InvitationTypes
        {
            Calendar = 1,
            Event = 2
        }
    }
}
