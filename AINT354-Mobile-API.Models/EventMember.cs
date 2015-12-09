using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.Models
{
    public class EventMember
    {
        public int Id { get; set; }

        [Required]
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
