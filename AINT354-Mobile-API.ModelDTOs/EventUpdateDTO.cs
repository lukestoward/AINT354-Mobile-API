using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class EventUpdateDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public bool AllDay { get; set; }

        [Required]
        public string StartDateTime { get; set; }

        [Required]
        public string EndDateTime { get; set; }

        [Required]
        public int ColourId { get; set; }
    }
}
