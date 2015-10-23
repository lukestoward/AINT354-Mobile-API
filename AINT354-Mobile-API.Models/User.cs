using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.Models
{
    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<Event> Events{ get; set; }
    }
}