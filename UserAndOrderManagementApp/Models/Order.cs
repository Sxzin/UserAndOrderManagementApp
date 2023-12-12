using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAndOrderManagement.Models
{
    public class Order
    {
        [Key]
/*        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]*/
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public string Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }

        public string UserName { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }

    }
}
