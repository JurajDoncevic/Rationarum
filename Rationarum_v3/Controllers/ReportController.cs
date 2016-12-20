using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Rationarum_v3.Models;
using Rationarum_v3.ViewModels;

namespace Rationarum_v3.Controllers
{
    //[Infrastructure.AllException]
    [HandleError]
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        //
        // GET: /Report/
        public ActionResult Index()
        {
            string currUserId = User.Identity.GetUserId();

            if (User.IsInRole("BlockedAssociationUser")) 
            {
                ViewBag.ReportType = new List<string>() { "Knjiga izlaznih računa", "Knjiga ulaznih računa", "Knjiga primitaka i izdataka", "Knjiga blagajne", "Popis dugotrajne nefinancijske imovine" }; 
            }
            else
            {
                ViewBag.ReportType = new List<string>() { "Godišnje izvješće", "Knjiga izlaznih računa", "Knjiga ulaznih računa", "Knjiga primitaka i izdataka", "Popis dugotrajne nefinancijske imovine", "Knjiga blagajne" }; 
            }

            List<string> documentedYears = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateExpenditure.Year.ToString()).Distinct().ToList();
            documentedYears.AddRange(ctx.Receipts.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateReceipt.Year.ToString()).Distinct().ToList());
            documentedYears.AddRange(ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateIngoingInvoice.Year.ToString()).Distinct().ToList());
            documentedYears.AddRange(ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateOutgoingInvoice.Year.ToString()).Distinct().ToList());
            documentedYears.AddRange(ctx.FixedAssets.Where(x => x.ApplicationUserId == currUserId).Select(x => x.DateFixedAsset.Year.ToString()).Distinct().ToList());
            documentedYears = documentedYears.Distinct().ToList();

            documentedYears.Add("Sve godine");

            ViewBag.DocumentedYears = documentedYears;

            return View(new ReportOptionViewModel() { ReportType = "Godišnje izvješće", ReportYear = "Sve godine" });
        }

        public ActionResult GenerateReport(ReportOptionViewModel reportOption)
        {
            if (reportOption.ReportType == "Knjiga izlaznih računa")
            {
                return RedirectToAction("OutgoingInvoices", new { year = reportOption.ReportYear });
            }
            else if(reportOption.ReportType == "Knjiga ulaznih računa")
            {
                return RedirectToAction("IngoingInvoices", new { year = reportOption.ReportYear });
            }
            else if (reportOption.ReportType == "Knjiga primitaka i izdataka")
            {
                return RedirectToAction("ExpendituresReceipts", new { year = reportOption.ReportYear });
            }
            else if (reportOption.ReportType == "Popis dugotrajne nefinancijske imovine")
            {
                return RedirectToAction("FixedAssets", new { year = reportOption.ReportYear });
            }
            else if (reportOption.ReportType == "Knjiga blagajne")
            {
                return RedirectToAction("Invoices", new { year = reportOption.ReportYear });
            }
            else if (reportOption.ReportType == "Godišnje izvješće" && reportOption.ReportYear != "Sve godine")
            {
                return RedirectToAction("YearlyReport", new { year = reportOption.ReportYear });
            }

            return RedirectToAction("Index");
        }

        public FileResult OutgoingInvoices(string year)
        {
            string currUserId = User.Identity.GetUserId();
            List<OutgoingInvoice> outgoingInvoices;

            if (year == "Sve godine")
            {
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
            }
            else
            {
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateOutgoingInvoice.Year.ToString() == year).ToList();
            }

            
            outgoingInvoices.Sort((x, y) => DateTime.Compare(x.DateOutgoingInvoice, y.DateOutgoingInvoice));


            string fileText = "<html><table border=\"1\"><tr><th>Redni broj</th><th>Broj računa</th><th>Datum</th><th>Podaci o kupcu</th><th>Ukupni iznos (s PDV-om)</th></tr>";

            int invoiceNumber = 0;

            foreach (OutgoingInvoice oi in outgoingInvoices)
            {
                fileText += "<tr>";
                fileText += "<td>" + (++invoiceNumber).ToString() + "</td>";
                fileText += "<td>" + oi.InvoiceClassNumber + "</td>";
                fileText += "<td>" + oi.DateOutgoingInvoice.ToShortDateString() + "</td>";
                fileText += "<td>" + oi.CustomerInfo + "</td>";
                fileText += "<td>" + oi.Amount.ToString() + "</td>";
                fileText += "</tr>";
            }


            fileText += "</table></html>";

            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "K_IR_" + associationName + "_" + year + ".html");
        }

        public FileResult IngoingInvoices(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<IngoingInvoice> ingoingInvoices;

            if (year == "Sve godine")
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
            }
            else
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateIngoingInvoice.Year.ToString() == year).ToList();
            }

            
            ingoingInvoices.Sort((x, y) => DateTime.Compare(x.DateIngoingInvoice, y.DateIngoingInvoice));


            string fileText = "<html><table border=\"1\"><tr><th>Redni broj</th><th>Broj računa</th><th>Datum</th><th>Podaci o dobavljaču</th><th>Ukupni iznos (s PDV-om)</th></tr>";

            int invoiceNumber = 0;

            foreach (IngoingInvoice ii in ingoingInvoices)
            {
                fileText += "<tr>";
                fileText += "<td>" + (++invoiceNumber).ToString() + "</td>";
                fileText += "<td>" + ii.InvoiceClassNumber + "</td>";
                fileText += "<td>" + ii.DateIngoingInvoice.ToShortDateString() + "</td>";
                fileText += "<td>" + ii.SupplierInfo + "</td>";
                fileText += "<td>" + ii.Amount.ToString() + "</td>";
                fileText += "</tr>";
            }


            fileText += "</table></html>";

            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "K_UR_" + associationName + "_" + year + ".html");
        }

        public FileResult ExpendituresReceipts(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<Expenditure> expenditures;
            List<Receipt> receipts;

            if (year == "Sve godine")
            {
                expenditures = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId).ToList();
                receipts = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId).ToList();
            }
            else
            {
                expenditures = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year.ToString() == year).ToList();
                receipts = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year.ToString() == year).ToList();
            }

            List<ExpenditureReceiptViewModel> expendituresReceipts = new List<ExpenditureReceiptViewModel>();

            List<ExpenditureViewModel> expendituresViewModel = new List<ExpenditureViewModel>();
            List<ReceiptViewModel> receiptsViewModel = new List<ReceiptViewModel>();

            foreach (Expenditure e in expenditures)
            {
                expendituresViewModel.Add(new ExpenditureViewModel() {
                    Id = e.IdExpenditure,
                    Date = e.DateExpenditure.ToShortDateString(),
                    JournalEntryNum = e.JournalEntryNum,
                    ApplicationUserId = e.ApplicationUserId,
                    AmountCash = e.AmountCash.ToString(),
                    AmountNonCashBenefit = e.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = e.AmountTransferAccount.ToString(),
                    Article22 = e.Article22.ToString(),
                    ValueAddedTax = e.ValueAddedTax.ToString(),
                    Totaled = e.Totaled
                    
                });
            }

            foreach (Receipt r in receipts)
            {
                receiptsViewModel.Add(new ReceiptViewModel()
                {
                    Id = r.IdReceipt,
                    Date = r.DateReceipt.ToShortDateString(),
                    JournalEntryNum = r.JournalEntryNum,
                    ApplicationUserId = r.ApplicationUserId,
                    AmountCash = r.AmountCash.ToString(),
                    AmountNonCashBenefit = r.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = r.AmountTransferAccount.ToString(),
                    ValueAddedTax = r.ValueAddedTax.ToString(),
                    Totaled = r.Totaled

                });
            }

            expendituresReceipts.AddRange(expendituresViewModel);
            expendituresReceipts.AddRange(receiptsViewModel);


            //mein gott
            expendituresReceipts.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.Date), Convert.ToDateTime(y.Date)));


            string fileText = "<html><table border=\"1\">";
            fileText += "<tr><th rowspan = \"2\">Redni broj</th><th rowspan = \"2\">Broj temeljnice</th><th rowspan = \"2\">Datum</th><th colspan = \"5\" rowspan = \"1\">Primici</th><th colspan = \"6\" rowspan = \"1\">Izdaci</th></tr>";
            fileText += "<tr><th>U gotovini</th><th>U naravi</th><th>Na žiro-račun</th><th>PDV</th><th>Ukupno</th><th>U gotovini</th><th>U naravi</th><th>Na žiro-račun</th><th>Članak 22</th><th>PDV</th><th>Ukupno</th></tr>";
            

            int expenditureNumber = 0;

            foreach (ExpenditureReceiptViewModel er in expendituresReceipts)
            {
                fileText += "<tr>";
                fileText += "<td>" + (++expenditureNumber).ToString() + "</td>";
                fileText += "<td>" + er.JournalEntryNum + "</td>";
                fileText += "<td>" + er.Date + "</td>";
                if (er is ReceiptViewModel)
                {
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountCash +"</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountNonCashBenefit + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountTransferAccount + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).ValueAddedTax + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).Totaled.ToString() + "</td>";

                    fileText += "<td></td><td></td><td></td><td></td><td></td><td></td>";
                }
                if (er is ExpenditureViewModel)
                {
                    fileText += "<td></td><td></td><td></td><td></td><td></td>";

                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountCash + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountNonCashBenefit + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountTransferAccount + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).Article22 + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).ValueAddedTax + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).Totaled.ToString() + "</td>";

                }
                
                
                
                fileText += "</tr>";
            }


            fileText += "</table></html>";

            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "K_PI_" + associationName + "_" + year + ".html");
        }


        [Authorize(Roles="AssociationUser")]
        public FileResult YearlyReport(string year)
        {            
            string currUserId = User.Identity.GetUserId();
            string fileText = "";

            int reportYear = Convert.ToInt32(year.Replace(".", ""));

            ApplicationUser user = ctx.Users.Where(x => x.Id == currUserId).First();

            //data about the association
            string asscociatonName = user.AssociationName;
            string adress = user.Adress;
            string oib = user.OIB;

            decimal sumReceiptsBefore;
            decimal sumExpendituresBefore;

            //calculations for previous years
            sumExpendituresBefore = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year < reportYear).Select(x => x.Totaled).DefaultIfEmpty(0).Sum();
            sumReceiptsBefore = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year < reportYear).Select(x => x.Totaled).DefaultIfEmpty(0).Sum();

            //calculations for the selected year
            //receipts
            decimal sumReceiptsCash = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year == reportYear).Select(x => x.AmountCash).DefaultIfEmpty(0).Sum();
            decimal sumReceiptsNonBenefit = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year == reportYear).Select(x => x.AmountNonCashBenefit).DefaultIfEmpty(0).Sum();
            decimal sumReceiptsTransfer = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year == reportYear).Select(x => x.AmountTransferAccount).DefaultIfEmpty(0).Sum();
            decimal sumReceiptsVAT = ctx.Receipts.Where(x => x.ApplicationUserId == currUserId && x.DateReceipt.Year == reportYear).Select(x => x.ValueAddedTax).DefaultIfEmpty(0).Sum();

            //expenditures
            decimal sumExpendituresCash = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year == reportYear).Select(x => x.AmountCash).DefaultIfEmpty(0).Sum();
            decimal sumExpendituresNonBenefit = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year == reportYear).Select(x => x.AmountNonCashBenefit).DefaultIfEmpty(0).Sum();
            decimal sumExpendituresTransfer = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year == reportYear).Select(x => x.AmountTransferAccount).DefaultIfEmpty(0).Sum();
            decimal sumExpendituresVAT = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year == reportYear).Select(x => x.ValueAddedTax).DefaultIfEmpty(0).Sum();
            decimal sumExpendituresArticle22 = ctx.Expenditures.Where(x => x.ApplicationUserId == currUserId && x.DateExpenditure.Year == reportYear).Select(x => x.Article22).DefaultIfEmpty(0).Sum();

            //totals of just the selected year
            decimal sumReceiptsNow = sumReceiptsCash + sumReceiptsNonBenefit + sumReceiptsTransfer - sumReceiptsVAT;
            decimal sumExpendituresNow = sumExpendituresCash + sumExpendituresNonBenefit + sumExpendituresTransfer - sumExpendituresArticle22 - sumExpendituresVAT;

            //total of all the years before and selected one
            decimal sumReceiptsNowBefore = sumReceiptsNow  + sumReceiptsBefore - sumExpendituresBefore;

            //THE ULTIMATE SUM GRAND TOTAL
            decimal sumTotal = sumReceiptsNowBefore - sumExpendituresNow;

            //font, encoding and title
            fileText += "<html style=\"font-family:Arial\" ><meta charset=\"UTF-8\"><h2 align = \"center\">PREGLED PRIMITAKA I IZDATAKA ZA <br>" + reportYear.ToString() + ". GODINU</br></h2>\n";
            
            //basic association data| name, serial num, etc.
            fileText += "<table align = \"center\">\n";
            fileText += "<tr><td align = \"left\"><b>NAZIV UDRUGE:</b></td><td align = \"center\">" + asscociatonName.ToUpper() + "</td></tr><tr><td align = \"left\"><b>ADRESA:</b></td><td align = \"center\">" + adress.ToUpper() + "</td></tr><tr><td align = \"left\"><b>OIB:</b></td><td align = \"center\">" + oib + "</td></tr>\n";
            fileText += "</table><br></br>\n";

            //receipts data table
            fileText += "<h3 align = \"center\">A) PRIMICI</h3><table align = \"center\">\n";
            fileText += "<tr><td align = \"left\">U GOTOVINI:</td><td align = \"right\">" + sumReceiptsCash.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">NA ŽIRO RAČUN:</td><td align = \"right\">" + sumReceiptsTransfer.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">U NARAVI:</td><td align = \"right\">" + sumReceiptsNonBenefit.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">PDV:</td><td align = \"right\">" + sumReceiptsVAT.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">UKUPNI PRIMICI:</td><td align = \"right\">" + sumReceiptsNow.ToString() + "</td></tr>\n";
            fileText += "</table><br></br>\n";


            //expenditure data table
            fileText += "<h3 align = \"center\">B) IZDACI</h3><table align = \"center\">\n";
            fileText += "<tr><td align = \"left\">U GOTOVINI:</td><td align = \"right\">" + sumExpendituresCash.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">NA ŽIRO RAČUN:</td><td align = \"right\">" + sumExpendituresTransfer.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">U NARAVI:</td><td align = \"right\">" + sumExpendituresNonBenefit.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">PDV:</td><td align = \"right\">" + sumExpendituresVAT.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">ČLANAK 22 ST 1.:</td><td align = \"right\">" + sumExpendituresArticle22.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">UKUPNI IZDACI:</td><td align = \"right\">" + sumExpendituresNow.ToString() + "</td></tr>\n";
            fileText += "</table><br></br>\n";


            //total table
            fileText += "<table align = \"center\">\n";
            fileText += "<tr><td align = \"left\">UKUPNI PRIMICI:</td><td align = \"right\">" + sumReceiptsNowBefore.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\">UKUPNI IZDACI:</td><td align = \"right\">" + sumExpendituresNow.ToString() + "</td></tr>\n";
            fileText += "<tr><td align = \"left\"><b>DOHODAK<b>:</td><td align = \"right\">" + sumTotal.ToString() + "</td></tr>\n";
            fileText += "</table><br></br>\n";

            //date and signature space; html tab close
            fileText += "<text align = \"left\">" + DateTime.Now.ToString("d.M.yyyy.") + "</text><br></br><text align = \"right\">Potpis ovlaštene osobe:</text>";
            fileText += "</html>";

            //mash it all into bytes
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            //sth extra for the filename
            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "G_I_" + associationName + "_" + reportYear.ToString() + ".html");
        }

        public FileResult FixedAssets(string year)
        {
            string currUserId = User.Identity.GetUserId();

            List<FixedAsset> fixedAssets;

            int count = 1;
            
            if (year == "Sve godine")
            {
                fixedAssets = ctx.FixedAssets.Where(x => x.ApplicationUserId == currUserId).ToList();
            }
            else
            {
                fixedAssets = ctx.FixedAssets.Where(x => x.ApplicationUserId == currUserId && x.DateFixedAsset.Year.ToString() == year).ToList();
            }

            fixedAssets.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.DateFixedAsset), Convert.ToDateTime(y.DateFixedAsset)));


            string fileText = "<html><table border=\"1\"><tr><th>Redni broj</th><th>Naziv stvari<br>ili prava</th><th>Datum nabave</th><th>Nabavna vrijednost</th><th>Knjigovodstvena vrijednost</th><th>Vijek trajanja</th><th>Stopa otpisa</th><th>Svota otpisa</th><th>Knjigovodstvena vrijednost na kraju godine</th></tr>\n";

            foreach (FixedAsset fa in fixedAssets)
            {
                fileText += "<tr>";
                fileText += "<td>" + (count++).ToString() + "</td>";
                fileText += "<td>" + fa.NameFixedAsset + "</td>";
                fileText += "<td>" + fa.DateFixedAsset.ToShortDateString() + "</td>";
                fileText += "<td>" + fa.PurchaseValue.ToString() + "</td>";
                fileText += "<td>" + fa.BookValue.ToString() + "</td>";
                fileText += "<td>" + fa.Lifetime + "</td>";
                fileText += "<td>" + fa.WriteDownRate.ToString() + "</td>";
                fileText += "<td>" + fa.WriteDownValue.ToString() + "</td>";
                fileText += "<td>" + fa.BookValueAtYearEnd.ToString() + "</td>";
                fileText += "</tr>\n";
            }

            fileText += "</table></html>";
            //mash it all into bytes
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            //sth extra for the filename
            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "DI_" + associationName + "_" + year.ToString() + ".html");
        }

        public FileResult Invoices(string year)
        {
            string currUserId = User.Identity.GetUserId();

            string fileText = "<html><table border=\"1\"><tr><th>Redni broj</th><th>Datum</th><th>Broj temeljnice</th><th>Opis</th><th>Primitak</th><th>Izdatak</th></tr>\n";

            List<InvoiceViewModel> invoices = new List<InvoiceViewModel>();
            List<IngoingInvoiceViewModel> ingoingInvoicesViewModel = new List<IngoingInvoiceViewModel>();
            List<OutgoingInvoiceViewModel> outgoingInvoicesViewModel = new List<OutgoingInvoiceViewModel>();
            List<IngoingInvoice> ingoingInvoices;
            List<OutgoingInvoice> outgoingInvoices;

            if (year == "Sve godine")
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId).ToList();
            }
            else
            {
                ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateIngoingInvoice.Year.ToString() == year).ToList();
                outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == currUserId && x.DateOutgoingInvoice.Year.ToString() == year).ToList();
            }

            foreach (OutgoingInvoice o in outgoingInvoices)
            {
                outgoingInvoicesViewModel.Add(new OutgoingInvoiceViewModel() {
                    Date = o.DateOutgoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = o.InvoiceClassNumber,
                    Amount = o.Amount.ToString(),
                    CustomerInfo = o.CustomerInfo

                });
            }

            foreach (IngoingInvoice i in ingoingInvoices)
            {
                ingoingInvoicesViewModel.Add(new IngoingInvoiceViewModel()
                {
                    Date = i.DateIngoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = i.InvoiceClassNumber,
                    Amount = i.Amount.ToString(),
                    SupplierInfo = i.SupplierInfo

                });
            }

            invoices.AddRange(ingoingInvoicesViewModel);
            invoices.AddRange(outgoingInvoicesViewModel);

            invoices.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.Date), Convert.ToDateTime(y.Date)));

            int count = 1;
            
            decimal invoiceExpenditures = ingoingInvoices.Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal invoiceReceipts = outgoingInvoices.Select(x => x.Amount).DefaultIfEmpty(0).Sum();


            
            foreach (InvoiceViewModel i in invoices)
            {
                fileText += "<tr>";
                fileText += "<td>" + (count++).ToString() + "</td>";
                fileText += "<td>" + i.Date + "</td>";
                fileText += "<td>" + i.InvoiceClassNumber + "</td>";
                if (i is IngoingInvoiceViewModel)
                { 
                    fileText += "<td>" + ((IngoingInvoiceViewModel)i).SupplierInfo + "</td>";
                    fileText += "<td></td>";
                    fileText += "<td>" + ((IngoingInvoiceViewModel)i).Amount + "</td>";  
                }
                if (i is OutgoingInvoiceViewModel)
                {
                    fileText += "<td>" + ((OutgoingInvoiceViewModel)i).CustomerInfo + "</td>";
                    fileText += "<td>" + ((OutgoingInvoiceViewModel)i).Amount + "</td>";
                    fileText += "<td></td>";             
                }
                fileText += "</tr>\n";

            }
            fileText += "</table><br>";
            fileText += "\n<br></br>Primici: " + invoiceReceipts.ToString();
            fileText += "\n<br></br>Izdaci: " + invoiceExpenditures.ToString();
            fileText += "\n<br></br>Dohodak: " + (invoiceReceipts-invoiceExpenditures).ToString();
            fileText += "</html>";

            //mash it all into bytes
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            //sth extra for the filename
            string associationName = ctx.Users.Where(x => x.Id == currUserId).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "KB_" + associationName + "_" + year.ToString() + ".html");
        }

        [Authorize(Roles = "Admin")]
        public FileResult AdminFullReport(string id)
        {
            string fileText = "";
            //EXPENDITURES AND RECEIPTS
            fileText += "<h3>Primici i izdaci</h3><br>";

            List<Expenditure> expenditures;
            List<Receipt> receipts;

            expenditures = ctx.Expenditures.Where(x => x.ApplicationUserId == id).ToList();
            receipts = ctx.Receipts.Where(x => x.ApplicationUserId == id).ToList();


            List<ExpenditureReceiptViewModel> expendituresReceipts = new List<ExpenditureReceiptViewModel>();

            List<ExpenditureViewModel> expendituresViewModel = new List<ExpenditureViewModel>();
            List<ReceiptViewModel> receiptsViewModel = new List<ReceiptViewModel>();

            foreach (Expenditure e in expenditures)
            {
                expendituresViewModel.Add(new ExpenditureViewModel()
                {
                    Id = e.IdExpenditure,
                    Date = e.DateExpenditure.ToShortDateString(),
                    JournalEntryNum = e.JournalEntryNum,
                    ApplicationUserId = e.ApplicationUserId,
                    AmountCash = e.AmountCash.ToString(),
                    AmountNonCashBenefit = e.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = e.AmountTransferAccount.ToString(),
                    Article22 = e.Article22.ToString(),
                    ValueAddedTax = e.ValueAddedTax.ToString(),
                    Totaled = e.Totaled

                });
            }

            foreach (Receipt r in receipts)
            {
                receiptsViewModel.Add(new ReceiptViewModel()
                {
                    Id = r.IdReceipt,
                    Date = r.DateReceipt.ToShortDateString(),
                    JournalEntryNum = r.JournalEntryNum,
                    ApplicationUserId = r.ApplicationUserId,
                    AmountCash = r.AmountCash.ToString(),
                    AmountNonCashBenefit = r.AmountNonCashBenefit.ToString(),
                    AmountTransferAccount = r.AmountTransferAccount.ToString(),
                    ValueAddedTax = r.ValueAddedTax.ToString(),
                    Totaled = r.Totaled

                });
            }

            expendituresReceipts.AddRange(expendituresViewModel);
            expendituresReceipts.AddRange(receiptsViewModel);


            //mein gott
            expendituresReceipts.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.Date), Convert.ToDateTime(y.Date)));


            fileText += "<html><table border=\"1\">";
            fileText += "<tr><th rowspan = \"2\">Redni broj</th><th rowspan = \"2\">Broj temeljnice</th><th rowspan = \"2\">Datum</th><th colspan = \"5\" rowspan = \"1\">Primici</th><th colspan = \"6\" rowspan = \"1\">Izdaci</th></tr>";
            fileText += "<tr><th>U gotovini</th><th>U naravi</th><th>Na žiro-račun</th><th>PDV</th><th>Ukupno</th><th>U gotovini</th><th>U naravi</th><th>Na žiro-račun</th><th>Članak 22</th><th>PDV</th><th>Ukupno</th></tr>";


            int expenditureNumber = 0;

            foreach (ExpenditureReceiptViewModel er in expendituresReceipts)
            {
                fileText += "<tr>";
                fileText += "<td>" + (++expenditureNumber).ToString() + "</td>";
                fileText += "<td>" + er.JournalEntryNum + "</td>";
                fileText += "<td>" + er.Date + "</td>";
                if (er is ReceiptViewModel)
                {
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountCash + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountNonCashBenefit + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).AmountTransferAccount + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).ValueAddedTax + "</td>";
                    fileText += "<td>" + ((ReceiptViewModel)er).Totaled.ToString() + "</td>";

                    fileText += "<td></td><td></td><td></td><td></td><td></td><td></td>";
                }
                if (er is ExpenditureViewModel)
                {
                    fileText += "<td></td><td></td><td></td><td></td><td></td>";

                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountCash + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountNonCashBenefit + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).AmountTransferAccount + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).Article22 + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).ValueAddedTax + "</td>";
                    fileText += "<td>" + ((ExpenditureViewModel)er).Totaled.ToString() + "</td>";

                }



                fileText += "</tr>";
            }


            fileText += "</table><br>";

            //FIXED ASSETS
            fileText += "<h3>Popis trajne nefinancijske imovine</h3><br>";
            
            List<FixedAsset> fixedAssets;

            int count = 1;


            fixedAssets = ctx.FixedAssets.Where(x => x.ApplicationUserId == id).ToList();
            

            fixedAssets.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.DateFixedAsset), Convert.ToDateTime(y.DateFixedAsset)));


            fileText += "<table border=\"1\"><tr><th>Redni broj</th><th>Naziv stvari<br>ili prava</th><th>Datum nabave</th><th>Nabavna vrijednost</th><th>Knjigovodstvena vrijednost</th><th>Vijek trajanja</th><th>Stopa otpisa</th><th>Svota otpisa</th><th>Knjigovodstvena vrijednost na kraju godine</th></tr>\n";

            foreach (FixedAsset fa in fixedAssets)
            {
                fileText += "<tr>";
                fileText += "<td>" + (count++).ToString() + "</td>";
                fileText += "<td>" + fa.NameFixedAsset + "</td>";
                fileText += "<td>" + fa.DateFixedAsset.ToShortDateString() + "</td>";
                fileText += "<td>" + fa.PurchaseValue.ToString() + "</td>";
                fileText += "<td>" + fa.BookValue.ToString() + "</td>";
                fileText += "<td>" + fa.Lifetime + "</td>";
                fileText += "<td>" + fa.WriteDownRate.ToString() + "</td>";
                fileText += "<td>" + fa.WriteDownValue.ToString() + "</td>";
                fileText += "<td>" + fa.BookValueAtYearEnd.ToString() + "</td>";
                fileText += "</tr>\n";
            }

            fileText += "</table><br>";

            //OUTGOING AND INGOING INVOICES
            fileText += "<h3>Ulazni i izlazni računi</h3><br>";

            fileText += "<table border=\"1\"><tr><th>Redni broj</th><th>Datum</th><th>Broj temeljnice</th><th>Opis</th><th>Primitak</th><th>Izdatak</th></tr>\n";

            List<InvoiceViewModel> invoices = new List<InvoiceViewModel>();
            List<IngoingInvoiceViewModel> ingoingInvoicesViewModel = new List<IngoingInvoiceViewModel>();
            List<OutgoingInvoiceViewModel> outgoingInvoicesViewModel = new List<OutgoingInvoiceViewModel>();
            List<IngoingInvoice> ingoingInvoices;
            List<OutgoingInvoice> outgoingInvoices;


            ingoingInvoices = ctx.IngoingInvoices.Where(x => x.ApplicationUserId == id).ToList();
            outgoingInvoices = ctx.OutgoingInvoices.Where(x => x.ApplicationUserId == id).ToList();


            foreach (OutgoingInvoice o in outgoingInvoices)
            {
                outgoingInvoicesViewModel.Add(new OutgoingInvoiceViewModel()
                {
                    Date = o.DateOutgoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = o.InvoiceClassNumber,
                    Amount = o.Amount.ToString(),
                    CustomerInfo = o.CustomerInfo

                });
            }

            foreach (IngoingInvoice i in ingoingInvoices)
            {
                ingoingInvoicesViewModel.Add(new IngoingInvoiceViewModel()
                {
                    Date = i.DateIngoingInvoice.ToShortDateString(),
                    InvoiceClassNumber = i.InvoiceClassNumber,
                    Amount = i.Amount.ToString(),
                    SupplierInfo = i.SupplierInfo

                });
            }

            invoices.AddRange(ingoingInvoicesViewModel);
            invoices.AddRange(outgoingInvoicesViewModel);

            invoices.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.Date), Convert.ToDateTime(y.Date)));

            count = 1;


            foreach (InvoiceViewModel i in invoices)
            {
                fileText += "<tr>";
                fileText += "<td>" + (count++).ToString() + "</td>";
                fileText += "<td>" + i.Date + "</td>";
                fileText += "<td>" + i.InvoiceClassNumber + "</td>";
                if (i is IngoingInvoiceViewModel)
                {
                    fileText += "<td>" + ((IngoingInvoiceViewModel)i).SupplierInfo + "</td>";
                    fileText += "<td></td>";
                    fileText += "<td>" + ((IngoingInvoiceViewModel)i).Amount + "</td>";
                }
                if (i is OutgoingInvoiceViewModel)
                {
                    fileText += "<td>" + ((OutgoingInvoiceViewModel)i).CustomerInfo + "</td>";
                    fileText += "<td>" + ((OutgoingInvoiceViewModel)i).Amount + "</td>";
                    fileText += "<td></td>";
                }
                fileText += "</tr>\n";

            }
            fileText += "</table><br></html>";


            //mash it all into bytes
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(fileText);

            //sth extra for the filename
            string associationName = ctx.Users.Where(x => x.Id == id).First().AssociationName;

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "SVE KNJIGE" + associationName + "_" + "SVE_GODINE.html");
        }
    }
}
