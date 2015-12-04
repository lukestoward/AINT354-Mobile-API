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
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public int ColourId { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
