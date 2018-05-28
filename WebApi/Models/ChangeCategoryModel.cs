using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ChangeCategoryModel
    {
        [Required]
        public int LotId { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}