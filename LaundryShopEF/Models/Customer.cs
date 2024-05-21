using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LaundryShopEF.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        public int PhoneNumber { get; set; }
        public string Gender { get; set; }
        public bool New { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }


        [NotMapped]
        [DataType(DataType.Upload)]
        [ScaffoldColumn(true)]

        public HttpPostedFileBase ImageUpload { get; set; }

        public virtual IList<Invoice> Invoices { get; set; }
    }
}