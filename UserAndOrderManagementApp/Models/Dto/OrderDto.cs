using System.ComponentModel.DataAnnotations.Schema;

namespace UserAndOrderManagement.Models.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
/*        public UserDto User { get; set; }
        public ProductDto Product { get; set; }*/
    }
}
