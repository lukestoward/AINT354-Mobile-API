using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class CommentDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        [Required]
        public int OrderNo { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
