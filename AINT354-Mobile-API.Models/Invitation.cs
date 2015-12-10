using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.Models
{
    public class Invitation
    {
        public Invitation()
        {
            CreatedDate = DateTime.Now;
            Responded = false;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        public User Sender { get; set; }

        [Required]
        public int RecipientId { get; set; }
        //public User Recipient { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool Responded { get; set; }

        [Required]
        public int TypeId { get; set; }
        public InvitationType Type { get; set; }

        public Guid? CalendarId { get; set; }
        public Guid? EventId { get; set; }

        [Required]
        public string DisplayMessage { get; set; }
    }
}
