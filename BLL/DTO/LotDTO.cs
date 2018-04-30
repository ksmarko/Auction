using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class LotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Img { get; set; }
        public double Price { get; set; }
        public DateTime TradeDuration { get; set; }
        public ICollection<CategoryDTO> Categories { get; set; }
        public UserDTO User { get; set; }

        public LotDTO()
        {
            Categories = new List<CategoryDTO>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
