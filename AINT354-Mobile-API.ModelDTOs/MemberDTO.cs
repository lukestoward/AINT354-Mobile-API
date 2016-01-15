using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.ModelDTOs
{
    public class MemberDTO
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
