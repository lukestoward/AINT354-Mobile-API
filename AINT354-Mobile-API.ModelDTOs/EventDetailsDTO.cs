using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AINT354_Mobile_API.Models;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class EventDetailsDTO
    {
        public string Id { get; set; }

        [Required]
        public string CreatorName { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Title { get; set; }

        public string Body { get; set; }

        public string Location { get; set; }

        [Required]
        public bool AllDay { get; set; }

        [Required]
        public string StartDateTime { get; set; }

        [Required]
        public string EndDateTime { get; set; }

        //public ICollection<EventComment> Comments { get; set; }

    }
}
