using DAL.Identity;
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

        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Lot> Lots { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public User()
        {
            Lots = new List<Lot>();
        }
    }
}
