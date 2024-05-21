using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LaundryShopEF.Models
{
    public class LundryContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public LundryContext() : base("LaundryDB")
        {

        }
    }
}