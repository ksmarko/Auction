using System.Collections.Generic;

namespace BLL.DTO
{
    public class LotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Img { get; set; }
        public double Price { get; set; }
        public int TradeDuration { get; set; }
        public bool IsVerified { get; set; }
        public ICollection<CategoryDTO> Categories { get; set; }
        public UserDTO User { get; set; }

        public LotDTO()
        {
            Categories = new List<CategoryDTO>();
            IsVerified = false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
