using System;
using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.Models
{
    public class EventComment : BaseEntity
    {
        public EventComment()
        {
            CreatedDate = DateTime.Now;
        }

        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int OrderNo { get; set; }

        [Required]
        public string Body { get; set; }

    }
}