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
    public class ReceiptController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        //
        // GET: /Receipt/
        public ActionResult Index(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<ReceiptViewModel> receiptsViewList = new List<ReceiptViewModel>();

            List<Receipt> receipts;

            List<string> documentedYears = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateReceipt.Year.ToString()).Distinct().ToList();
            documentedYears.Add("Bez filtra");

            if (!documentedYears.Contains(year) || year == "Bez filtra")
            {
                receipts = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId).ToList();
                ViewBag.SelectedValue = "Bez filtra";
            }
            else
            {
                receipts = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year.ToString() == year).ToList();
                ViewBag.SelectedValue = year;
            }


            receipts.Sort((x, y) => DateTime.Compare(x.DateReceipt, y.DateReceipt));

            foreach (Receipt e in receipts)
            {
                receiptsViewList.Add(new ReceiptViewModel
                {
                    Id = e.IdReceipt,
                    JournalEntryNum = e.JournalEntryNum,
                    Date = e.DateReceipt.ToShortDateString(),
                    AmountCash = e.AmountCash.ToString(),
                    AmountNonCashBenefit = e.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = e.AmountTransferAccount.ToString(),
                    ValueAddedTax = e.ValueAddedTax.ToString(),
                    Totaled = e.Totaled
                });
            }

            ViewBag.DocumentedYears = documentedYears;

            return View(receiptsViewList);
        }


        //
        // GET: /Receipt/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Receipt/Create
        [HttpPost]
        public ActionResult Create(ReceiptViewModel receiptView)
        {
            try
            {
                // TODO: Add insert logic here
                string currUser = User.Identity.GetUserId();

                decimal amountCash = Convert.ToDecimal(receiptView.AmountCash);
                decimal amountNonCashBenefit = Convert.ToDecimal(receiptView.AmountNonCashBenefit);
                decimal amountTransferAccount = Convert.ToDecimal(receiptView.AmountTransferAccount);
                decimal valueAddedTax = Convert.ToDecimal(receiptView.ValueAddedTax);


                decimal totaled = amountCash + amountNonCashBenefit + amountTransferAccount - valueAddedTax;

                DateTime date = Convert.ToDateTime(receiptView.Date);

                ctx.Receipts.Add(new Receipt
                {
                    JournalEntryNum = receiptView.JournalEntryNum,
                    DateReceipt = date,
                    AmountCash = amountCash,
                    AmountNonCashBenefit = amountNonCashBenefit,
                    AmountTransferAccount = amountTransferAccount,
                    ValueAddedTax = valueAddedTax,
                    Totaled = totaled,
                    ApplicationUserId = currUser
                });

                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // GET: /Receipt/Edit/5
        public ActionResult Edit(int id)
        {
            Receipt receipt = ctx.Receipts.Where(x => x.IdReceipt == id).First();

            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (receipt.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            ReceiptViewModel receiptView = new ReceiptViewModel()
            {
                Id = receipt.IdReceipt,
                JournalEntryNum = receipt.JournalEntryNum,
                Date = receipt.DateReceipt.ToShortDateString(),
                AmountCash = receipt.AmountCash.ToString(),
                AmountNonCashBenefit = receipt.AmountNonCashBenefit.ToString(),
                AmountTransferAccount = receipt.AmountTransferAccount.ToString(),
                ValueAddedTax = receipt.ValueAddedTax.ToString(),
                Totaled = receipt.Totaled
            };
            return View(receiptView);
        }

        //
        // POST: /Receipt/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ReceiptViewModel receiptView)
        {

            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ctx.Receipts.Where(x => x.IdReceipt == id).First().ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add update logic here
                

                decimal amountCash = Convert.ToDecimal(receiptView.AmountCash);
                decimal amountNonCashBenefit = Convert.ToDecimal(receiptView.AmountNonCashBenefit);
                decimal amountTransferAccount = Convert.ToDecimal(receiptView.AmountTransferAccount);
                decimal valueAddedTax = Convert.ToDecimal(receiptView.ValueAddedTax);


                decimal totaled = amountCash + amountNonCashBenefit + amountTransferAccount - valueAddedTax;

                DateTime date = Convert.ToDateTime(receiptView.Date);

                ctx.Receipts.Where(x => x.IdReceipt == id).First().JournalEntryNum = receiptView.JournalEntryNum;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().DateReceipt = date;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().ApplicationUserId = currUserId;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().AmountCash = amountCash;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().AmountNonCashBenefit = amountNonCashBenefit;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().AmountTransferAccount = amountTransferAccount;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().ValueAddedTax = valueAddedTax;
                ctx.Receipts.Where(x => x.IdReceipt == id).First().Totaled = totaled;

                ctx.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // POST: /Receipt/Delete/5
        public ActionResult Delete(int id)
        {
            Receipt receipt = ctx.Receipts.Where(x => x.IdReceipt == id).First();
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (receipt.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add delete logic here
                ctx.Receipts.Remove(receipt);
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
