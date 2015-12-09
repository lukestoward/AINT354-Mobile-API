using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class InvitationDTO
    {
        public string Id { get; set; }

        public string From { get; set; }

        public string DateSent { get; set; }

        public string Type { get; set; }
    }
}
