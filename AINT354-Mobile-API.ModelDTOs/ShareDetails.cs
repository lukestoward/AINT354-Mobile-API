using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class ShareDetails
    {
        [Required]
        public int SenderUserId { get; set; }

        [Required]
        public string Email { get; set; }

        public string CalendarId { get; set; }

        public string EventId { get; set; }

    }
}
