using System.Collections.Generic;

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
