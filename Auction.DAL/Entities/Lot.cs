using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class Lot
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Img { get; set; }

        public double Price { get; set; }

        public int TradeDuration { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }

        public bool IsVerified { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public Lot()
        {
            IsVerified = false;
        }
    }
}
