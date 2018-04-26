using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<LotDTO> Lots { get; set; }

        public CategoryDTO()
        {
            Lots = new List<LotDTO>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
