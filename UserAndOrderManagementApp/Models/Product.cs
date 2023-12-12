using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserAndOrderManagement.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }

    }
}
