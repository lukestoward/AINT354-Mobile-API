using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.Models
{
    public class Calendar : BaseEntity
    {
        public Calendar()
        {
            CreatedDate = DateTime.Now;
            Events = new List<Event>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ColourId { get; set; }
        public virtual Colour Colour { get; set; }

        [Required]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public int TypeId { get; set; }
        public virtual CalendarType Type { get; set; }

        public virtual ICollection<Event> Events { get; set; }

    }
}
