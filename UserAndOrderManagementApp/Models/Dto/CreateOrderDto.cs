using System.ComponentModel.DataAnnotations;
using UserAndOrderManagement.Models;

namespace UserAndOrderManagementApp.Models.Dto
{
    public class CreateOrderDto
    {
        public string Id { get; set; }
        public int ProductId { get; set; }
    }
}
