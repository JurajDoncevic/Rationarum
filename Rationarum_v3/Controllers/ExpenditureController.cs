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
    public class ExpenditureController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        //
        // GET: /Expenditure/
        public ActionResult Index(string year)
        {

            string currUserId = User.Identity.GetUserId();

            List<ExpenditureViewModel> expendituresViewList = new List<ExpenditureViewModel>();

            List<Expenditure> expenditures;

            List<string> documentedYears = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateExpenditure.Year.ToString()).Distinct().ToList();
            documentedYears.Add("Bez filtra");

            if (!documentedYears.Contains(year) || year == "Bez filtra")
            {
                expenditures = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId).ToList();
                ViewBag.SelectedValue = "Bez filtra";
            }
            else
            {
                expenditures = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year.ToString() == year).ToList();
                ViewBag.SelectedValue = year;
            }


            expenditures.Sort((x, y) => DateTime.Compare(x.DateExpenditure, y.DateExpenditure));

            foreach (Expenditure e in expenditures)
            {
                expendituresViewList.Add(new ExpenditureViewModel
                {
                    Id = e.IdExpenditure,
                    JournalEntryNum = e.JournalEntryNum,
                    Date = e.DateExpenditure.ToShortDateString(),
                    AmountCash = e.AmountCash.ToString(),
                    AmountNonCashBenefit = e.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = e.AmountTransferAccount.ToString(),
                    Article22 = e.Article22.ToString(),
                    ValueAddedTax = e.ValueAddedTax.ToString(),
                    Totaled = e.Totaled
                });
            }


            ViewBag.DocumentedYears = documentedYears;

            return View(expendituresViewList);
        }

        //
        // GET: /Expenditure/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Expenditure/Create
        [HttpPost]
        public ActionResult Create(ExpenditureViewModel expenditureView)
        {
            try
            {
                // TODO: Add insert logic here
                string currUser = User.Identity.GetUserId();

                decimal amountCash = Convert.ToDecimal(expenditureView.AmountCash);
                decimal amountNonCashBenefit = Convert.ToDecimal(expenditureView.AmountNonCashBenefit);
                decimal amountTransferAccount = Convert.ToDecimal(expenditureView.AmountTransferAccount);
                decimal article22 = Convert.ToDecimal(expenditureView.Article22);
                decimal valueAddedTax = Convert.ToDecimal(expenditureView.ValueAddedTax);


                decimal totaled = amountCash + amountNonCashBenefit + amountTransferAccount - article22 - valueAddedTax;

                DateTime date = Convert.ToDateTime(expenditureView.Date);

                ctx.Expenditures.Add(new Expenditure 
                {
                    JournalEntryNum = expenditureView.JournalEntryNum,
                    DateExpenditure = date,
                    AmountCash = amountCash,
                    AmountNonCashBenefit = amountNonCashBenefit,
                    AmountTransferAccount = amountTransferAccount,
                    Article22 = article22,
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
        // GET: /Expenditure/Edit/5
        public ActionResult Edit(int id)
        {
            Expenditure expenditure = ctx.Expenditures.Where(x => x.IdExpenditure == id).First();
            
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (expenditure.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }
            
            ExpenditureViewModel expenditureView = new ExpenditureViewModel() 
            {
                Id = expenditure.IdExpenditure,
                JournalEntryNum = expenditure.JournalEntryNum,
                Date = expenditure.DateExpenditure.ToShortDateString(),
                AmountCash = expenditure.AmountCash.ToString(),
                AmountNonCashBenefit = expenditure.AmountNonCashBenefit.ToString(),
                AmountTransferAccount = expenditure.AmountTransferAccount.ToString(),
                Article22 = expenditure.Article22.ToString(),
                ValueAddedTax = expenditure.ValueAddedTax.ToString(),
                Totaled = expenditure.Totaled
            };
            return View(expenditureView);
        }

        //
        // POST: /Expenditure/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ExpenditureViewModel expenditureView)
        {
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ctx.Expenditures.Where(x => x.IdExpenditure == id).First().ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add update logic here
                
                
                decimal amountCash = Convert.ToDecimal(expenditureView.AmountCash);
                decimal amountNonCashBenefit = Convert.ToDecimal(expenditureView.AmountNonCashBenefit);
                decimal amountTransferAccount = Convert.ToDecimal(expenditureView.AmountTransferAccount);
                decimal article22 = Convert.ToDecimal(expenditureView.Article22);
                decimal valueAddedTax = Convert.ToDecimal(expenditureView.ValueAddedTax);


                decimal totaled = amountCash + amountNonCashBenefit + amountTransferAccount - article22 - valueAddedTax;

                DateTime date = Convert.ToDateTime(expenditureView.Date);

                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().JournalEntryNum = expenditureView.JournalEntryNum;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().DateExpenditure = date;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().ApplicationUserId = currUserId;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().AmountCash = amountCash;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().AmountNonCashBenefit = amountNonCashBenefit;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().AmountTransferAccount = amountTransferAccount;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().Article22 = article22;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().ValueAddedTax = valueAddedTax;
                ctx.Expenditures.Where(x => x.IdExpenditure == id).First().Totaled = totaled;

                ctx.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // POST: /Expenditure/Delete/5
        public ActionResult Delete(int id)
        {
            Expenditure expenditure = ctx.Expenditures.Where(x => x.IdExpenditure == id).First();
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (expenditure.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }
            try
            {
                // TODO: Add delete logic here
                
                ctx.Expenditures.Remove(expenditure);
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
