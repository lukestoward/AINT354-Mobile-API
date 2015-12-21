using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class DeleteCommentDTO
    {
        public int UserId { get; set; }

        public Guid EventId { get; set; }

        public int CommentId { get; set; }
    }
}
