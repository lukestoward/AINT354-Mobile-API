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

        [Required]
        public List<int> MemberIds { get; set; }
    }
}
