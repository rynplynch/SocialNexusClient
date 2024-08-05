using System.ComponentModel.DataAnnotations;

namespace SocialNexusClient.ViewModels
{
    public class ProductViewModel
    {
        public int PrimaryKey { get; set; }

        public string? OwnerId { get; set; }

        [StringLength(15)]
        [Required(AllowEmptyStrings = false)]
        public required string Title { get; set; }

        [Required(ErrorMessage = "This field can not be empty.")]
        public required string Description { get; set; }
    }
}
