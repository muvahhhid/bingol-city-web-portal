using System.ComponentModel.DataAnnotations;

namespace MyCityProject.Models
{
    public class District
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "District name is required.")]
        [StringLength(100, ErrorMessage = "District name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image path is required.")]
        [StringLength(300, ErrorMessage = "Image path cannot be longer than 300 characters.")]
        public string ImagePath { get; set; } = string.Empty;
    }
}