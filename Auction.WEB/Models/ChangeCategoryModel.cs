using System.ComponentModel.DataAnnotations;

namespace Auction.WEB.Models
{
    public class ChangeCategoryModel
    {
        [Required]
        public int LotId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}