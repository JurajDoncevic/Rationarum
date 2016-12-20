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
    public class OutgoingInvoiceController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        
        //
        // GET: /OutgoingInvoice/
        public ActionResult Index(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<OutgoingInvoiceViewModel> outgoingInvoicesViewList = new List<OutgoingInvoiceViewModel>();
            
            List<OutgoingInvoice> outgoingInvoices;

            List<string> documentedYears = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateOutgoingInvoice.Year.ToString()).Distinct().ToList();
            documentedYears.Add("Bez filtra");

            if (!documentedYears.Contains(year) || year == "Bez filtra")
            {
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
                ViewBag.SelectedValue = "Bez filtra";
            }
            else
            {
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateOutgoingInvoice.Year.ToString() == year).ToList();
                ViewBag.SelectedValue = year;
            }



            outgoingInvoices.Sort((x, y) => DateTime.Compare(x.DateOutgoingInvoice, y.DateOutgoingInvoice));

            foreach (OutgoingInvoice outgoingInvoice in outgoingInvoices)
            {
                outgoingInvoicesViewList.Add(new OutgoingInvoiceViewModel
                {
                    Id = outgoingInvoice.IdOutgoingInvoice,
                    Date = outgoingInvoice.DateOutgoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = outgoingInvoice.InvoiceClassNumber,
                    CustomerInfo = outgoingInvoice.CustomerInfo,
                    Amount = outgoingInvoice.Amount.ToString()
                });
            }


            ViewBag.DocumentedYears = documentedYears;
            

            return View(outgoingInvoicesViewList);
        }


        //
        // GET: /OutgoingInvoice/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OutgoingInvoice/Create
        [HttpPost]
        public ActionResult Create(OutgoingInvoiceViewModel outgoingInvoiceView)
        {
            try
            {
                // TODO: Add insert logic here
                string currUserId = User.Identity.GetUserId();

                decimal amount = Convert.ToDecimal(outgoingInvoiceView.Amount);
                DateTime date = Convert.ToDateTime(outgoingInvoiceView.Date);

                OutgoingInvoice outgoingInvoice = new OutgoingInvoice()
                {
                    ApplicationUserId = currUserId,
                    DateOutgoingInvoice = date,
                    InvoiceClassNumber = outgoingInvoiceView.InvoiceClassNumber,
                    CustomerInfo = outgoingInvoiceView.CustomerInfo,
                    Amount = amount
                };


                ctx.OutgoingInvoices.Add(outgoingInvoice);
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // GET: /OutgoingInvoice/Edit/5
        public ActionResult Edit(int id)
        {
            OutgoingInvoice outgoingInvoice = ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First();
            
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (outgoingInvoice.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }


            OutgoingInvoiceViewModel outgoingInvoiceView = new OutgoingInvoiceViewModel()
            {
                Id = outgoingInvoice.IdOutgoingInvoice,
                ApplicationUserId = outgoingInvoice.ApplicationUserId,
                Date = outgoingInvoice.DateOutgoingInvoice.ToShortDateString(),
                InvoiceClassNumber = outgoingInvoice.InvoiceClassNumber,
                CustomerInfo = outgoingInvoice.CustomerInfo,
                Amount = outgoingInvoice.Amount.ToString()
            };

            return View(outgoingInvoiceView);
        }

        //
        // POST: /OutgoingInvoice/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, OutgoingInvoiceViewModel outgoingInvoiceView)
        {
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ctx.OutgoingInvoices.Where(x => x.IdOutgoingInvoice == id).First().ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add update logic here

                ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First().InvoiceClassNumber = outgoingInvoiceView.InvoiceClassNumber;
                ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First().DateOutgoingInvoice = Convert.ToDateTime(outgoingInvoiceView.Date);
                ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First().CustomerInfo = outgoingInvoiceView.CustomerInfo;
                ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First().Amount = Convert.ToDecimal(outgoingInvoiceView.Amount);
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // POST: /OutgoingInvoice/Delete/5
        public ActionResult Delete(int id)
        {
            OutgoingInvoice outgoingInvoice = ctx.OutgoingInvoices.Where(o => o.IdOutgoingInvoice == id).First();
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (outgoingInvoice.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }
            try
            {
                // TODO: Add delete logic here
                
                ctx.OutgoingInvoices.Remove(outgoingInvoice);


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
