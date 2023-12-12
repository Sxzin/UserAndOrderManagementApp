using UserAndOrderManagement.Models;

namespace UserAndOrderManagementApp.Models.Dto
{
    public class UpdateOrderDto
    {
        public string Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
