using System.ComponentModel.DataAnnotations;

namespace CoreApp.API.Models
{
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage = "Name filed are required!")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}