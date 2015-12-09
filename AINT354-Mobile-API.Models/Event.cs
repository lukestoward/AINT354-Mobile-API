using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AINT354_Mobile_API.Models
{
    public class Event
    {
        public Event()
        {
            CreatedDate = DateTime.Now;
            Comments = new List<EventComment>();
            Members = new List<EventMember>();
        }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [Required]
        public Guid CalendarId { get; set; }
        public Calendar Calendar { get; set; }

        [Required]
        public int CreatorId { get; set; }
        public User Creator { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Title { get; set; }

        public string Body { get; set; }
    
        public string Location { get; set; }

        [Required]
        public bool AllDay { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }
        
        public ICollection<EventComment> Comments { get; set; }

        public ICollection<EventMember> Members { get; set; }
    }
}