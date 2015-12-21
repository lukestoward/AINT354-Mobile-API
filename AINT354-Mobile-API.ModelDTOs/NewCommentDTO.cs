using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class NewCommentDTO
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
