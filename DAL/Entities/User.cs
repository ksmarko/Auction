using DAL.Identity.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class User
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public User() => Lots = new List<Lot>();
    }
}
