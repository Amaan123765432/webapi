using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ProdID { get; set; }

        public string ProdName { get; set; }

        public float ProdPrice {  get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual catogory? Catogory { get; set; }
    }
}
