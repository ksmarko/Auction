using Auction.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual User User { get; set; }
    }
}
