using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class EventMembersDTO
    {
        [Required]
        public Guid EventId { get; set; }

        //Needs to be a string for some reason, as the android app 
        //Can't send a List<int>... Stupid
        [Required]
        public string MemberIds { get; set; }
    }
}
