using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class CalendarDTO
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public string Type { get; set; }

        //public ICollection<Event> Events { get; set; }
    }
}
