using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.Models
{
    public class CalendarMember
    {
        public int Id { get; set; }

        [Required]
        public Guid CalendarId { get; set; }
        public Calendar Calendar { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
