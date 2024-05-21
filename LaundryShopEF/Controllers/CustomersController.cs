using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryShopEF.Models;

namespace LaundryShopEF.Controllers
{
    public class CustomersController : Controller
    {
        private LundryContext db = new LundryContext();

        // GET: Customers
        public ActionResult Index()
        {
           var data = db.Customers.Include(e => e.Invoices).ToList();

            if (Request.IsAjaxRequest())

                return PartialView(data);

            return View(data);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer, string command = "")
        {
            if (customer.ImageUpload != null)
            {
                customer.ImageUrl = "/Images/" + Guid.NewGuid() + Path.GetExtension(customer.ImageUpload.FileName);

                customer.ImageUpload.SaveAs(Server.MapPath(customer.ImageUrl));

                TempData["ImageUrl"] = customer.ImageUrl;
            }
            else
            {
                customer.ImageUrl = TempData["ImageUrl"]?.ToString();
            }
            if (command == "add")
            {
                if (customer.Invoices is null)
                    customer.Invoices = new List<Invoice>();
                customer.Invoices.Add(new Invoice());
                return View(customer);
            }

            else if (command.StartsWith("delete"))
            {
                int index = Convert.ToInt32(command.Replace("delete-", string.Empty));
                customer.Invoices.RemoveAt(index);
                return View(customer);
            }
            if (ModelState.IsValid)
            {


                db.Customers.Add(customer);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        [ChildActionOnly]
        public ActionResult InvoiceEntry(Customer customer)
        {
            if (customer is null)
                customer = new Customer();
            if (customer.Invoices is null)
                customer.Invoices = new List<Invoice>();

            return PartialView("InvoiceEntry", customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Include(e => e.Invoices).FirstOrDefault(e => e.CustomerID == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Name,Address,PhoneNumber,Gender,New,ImageUrl,ImageUpload,Invoices")] Customer customer, string command = "")
        {
            if (command == "add")
            {
                if (customer.Invoices is null)
                    customer.Invoices = new List<Invoice>();
                customer.Invoices.Add(new Invoice());
                return View(customer);
            }

            else if (command.StartsWith("delete"))
            {
                int index = Convert.ToInt32(command.Replace("delete-", string.Empty));
                customer.Invoices.RemoveAt(index);
                return View(customer);
            }
            if (ModelState.IsValid)
            {
                if (customer.ImageUpload != null)
                {
                    customer.ImageUrl = "/Images/" + Guid.NewGuid() + Path.GetExtension(customer.ImageUpload.FileName);

                    customer.ImageUpload.SaveAs(Server.MapPath(customer.ImageUrl));
                }


                try
                {

                    db.Invoices.RemoveRange(db.Invoices.Where(i => i.CustomerID == customer.CustomerID));
                    db.SaveChanges();

                    foreach (var Inv in customer.Invoices)
                    {
                        Invoice InvEntry = new Invoice();
                        InvEntry.CustomerID = customer.CustomerID;
                        InvEntry.OrderDate = Inv.OrderDate;
                        InvEntry.LaundryType = Inv.LaundryType;
                        InvEntry.ItemName = Inv.ItemName;
                        InvEntry.Price = Inv.Price;
                        InvEntry.Qty = Inv.Qty;

                        db.Invoices.Add(InvEntry);
                        db.SaveChanges();
                    }

                    Customer customerEdit = db.Customers.Find(customer.CustomerID);
                    customerEdit.Name = customer.Name;
                    customerEdit.Address = customer.Address;
                    customerEdit.PhoneNumber = customer.PhoneNumber;
                    customerEdit.Gender = customer.Gender;
                    customerEdit.New = customer.New;
                    customerEdit.ImageUrl = customer.ImageUrl;

                    db.Entry(customerEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception err)
                {
                    ModelState.AddModelError("save", err.Message);
                    return View(customer);
                }
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult AjaxDelete(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
