using System.ComponentModel.DataAnnotations;

namespace AINT354_Mobile_API.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; } 
    }
}