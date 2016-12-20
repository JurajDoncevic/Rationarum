using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rationarum_v3.Models;
using Rationarum_v3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rationarum_v3.Controllers
{
    //[Infrastructure.AllException]
    [HandleError]
    [Authorize(Roles = "AssociationUser")]
    public class IngoingInvoiceController : Controller
    {
        ApplicationDbContext ctx = new ApplicationDbContext();

        //
        // GET: /IngoingInvoice/
        public ActionResult Index(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<IngoingInvoiceViewModel> ingoingInvoicesViewList = new List<IngoingInvoiceViewModel>();
            List<IngoingInvoice> ingoingInvoices;

            List<string> documentedYears = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateIngoingInvoice.Year.ToString()).Distinct().ToList();
            documentedYears.Add("Bez filtra");

            if (!documentedYears.Contains(year) || year == "Bez filtra")
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
                ViewBag.SelectedValue = "Bez filtra";
            }
            else
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateIngoingInvoice.Year.ToString() == year).ToList();
                ViewBag.SelectedValue = year;
            }

            ingoingInvoices.Sort((x, y) => DateTime.Compare(x.DateIngoingInvoice, y.DateIngoingInvoice));

            foreach (IngoingInvoice ingoingInvoice in ingoingInvoices)
            {
                ingoingInvoicesViewList.Add(new IngoingInvoiceViewModel 
                {
                    Id = ingoingInvoice.IdIngoingInvoice,
                    Date = ingoingInvoice.DateIngoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = ingoingInvoice.InvoiceClassNumber,
                    SupplierInfo = ingoingInvoice.SupplierInfo,
                    Amount = ingoingInvoice.Amount.ToString()
                });
            }

            ViewBag.DocumentedYears = documentedYears;

            return View(ingoingInvoicesViewList);
        }


        //
        // GET: /IngoingInvoice/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /IngoingInvoice/Create
        [HttpPost]
        public ActionResult Create(IngoingInvoiceViewModel ingoingInvoiceView)
        {
            try
            {
                // TODO: Add insert logic here
                string currUserId = User.Identity.GetUserId();

                decimal amount = Convert.ToDecimal(ingoingInvoiceView.Amount);
                DateTime date = Convert.ToDateTime(ingoingInvoiceView.Date);

                IngoingInvoice ingoingInvoice = new IngoingInvoice()
                {
                    ApplicationUserId = currUserId,
                    DateIngoingInvoice = date,
                    InvoiceClassNumber = ingoingInvoiceView.InvoiceClassNumber,
                    SupplierInfo = ingoingInvoiceView.SupplierInfo,
                    Amount = amount
                };


                ctx.IngoingInvoices.Add(ingoingInvoice);
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // GET: /IngoingInvoice/Edit/5
        public ActionResult Edit(int id)
        {
            IngoingInvoice ingoingInvoice = ctx.IngoingInvoices.Where(i => i.IdIngoingInvoice == id).First();

            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ingoingInvoice.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            IngoingInvoiceViewModel ingoingInvoiceView = new IngoingInvoiceViewModel() 
            {
                Id = ingoingInvoice.IdIngoingInvoice,
                ApplicationUserId = ingoingInvoice.ApplicationUserId,
                Date = ingoingInvoice.DateIngoingInvoice.ToShortDateString(),
                InvoiceClassNumber = ingoingInvoice.InvoiceClassNumber,
                SupplierInfo = ingoingInvoice.SupplierInfo,
                Amount = ingoingInvoice.Amount.ToString()
            };

            return View(ingoingInvoiceView);
        }

        //
        // POST: /IngoingInvoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, IngoingInvoiceViewModel ingoingInvoiceView)
        {
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ctx.IngoingInvoices.Where(x => x.IdIngoingInvoice == id).First().ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add update logic here
                ctx.IngoingInvoices.Where(o => o.IdIngoingInvoice == id).First().InvoiceClassNumber = ingoingInvoiceView.InvoiceClassNumber;
                ctx.IngoingInvoices.Where(o => o.IdIngoingInvoice == id).First().DateIngoingInvoice = Convert.ToDateTime(ingoingInvoiceView.Date);
                ctx.IngoingInvoices.Where(o => o.IdIngoingInvoice == id).First().SupplierInfo = ingoingInvoiceView.SupplierInfo;
                ctx.IngoingInvoices.Where(o => o.IdIngoingInvoice == id).First().Amount = Convert.ToDecimal(ingoingInvoiceView.Amount);
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }


        //
        // POST: /IngoingInvoice/Delete/5
        public ActionResult Delete(int id)
        {
            IngoingInvoice ingoingInvoice = ctx.IngoingInvoices.Where(o => o.IdIngoingInvoice == id).First();
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ingoingInvoice.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }
            try
            {
                // TODO: Add delete logic here
                
                ctx.IngoingInvoices.Remove(ingoingInvoice);


                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }
    }
}
