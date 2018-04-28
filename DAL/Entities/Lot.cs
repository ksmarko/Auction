using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Img { get; set; }
        public double Price { get; set; }
        public DateTime TradeDuration { get; set; }//Date of trade end
        public DateTime? StartTrade { get; set; }//Date of trade start, set by admin/moderator
        public virtual ICollection<Category> Categories { get; set; }
        public string UserId { get; set; }
        public string WinUserID { get; set; }
        public virtual User User { get; set; }
        public virtual User WinUser { get; set; }

        public Lot()
        {
            Categories = new List<Category>();
        }
    }
}
