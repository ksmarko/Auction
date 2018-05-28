using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.DAL.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }

        public Category()
        {
            Lots = new List<Lot>();
        }
    }
}
