using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LaundryShopEF.Models
{
    public class Invoice
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [DisplayName("Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public InvoiceType LaundryType { get; set; }

        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public enum InvoiceType
    {
        Press,
        Wash,
        DryWash
    }
}