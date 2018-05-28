using System.ComponentModel.DataAnnotations;

namespace Auction.WEB.Models
{
    public class EditRoleModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}