using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class InviteRSVP
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public bool Accept { get; set; }
    }
}
