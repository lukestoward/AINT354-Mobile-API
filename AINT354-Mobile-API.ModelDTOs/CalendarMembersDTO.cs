using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class CalendarMembersDTO
    {
        [Required]
        public Guid CalendarId { get; set; }

        //Needs to be a string for some reason, as the android app 
        //Can't send a List<int>... Stupid
        [Required]
        public string MemberIds { get; set; }
    }
}
