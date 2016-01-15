using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class EventMembersDTO
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public List<int> MemberIds { get; set; }
    }
}
