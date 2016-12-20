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
    [HandleError]
    [Authorize(Roles = "AssociationUser")]
    public class FixedAssetController : Controller
    {

        private ApplicationDbContext ctx = new ApplicationDbContext();

        //
        // GET: /FixedAsset/
        public ActionResult Index()
        {
            string currUserId = User.Identity.GetUserId();
            List<FixedAsset> fixedAssets = ctx.FixedAssets.Where(x => x.ApplicationUserId == currUserId).ToList();
            List<FixedAssetViewModel> fixedAssetsViewModel = new List<FixedAssetViewModel>();

            fixedAssets.Sort((x, y) => DateTime.Compare(x.DateFixedAsset, y.DateFixedAsset));

            foreach (FixedAsset fa in fixedAssets)
            {
                fixedAssetsViewModel.Add(new FixedAssetViewModel()
                {
                    Id = fa.IdFixedAsset,
                    ApplicationUserId = fa.ApplicationUserId,
                    Name = fa.NameFixedAsset,
                    Date = fa.DateFixedAsset.ToShortDateString(),
                    PurchaseValue = fa.PurchaseValue.ToString(),
                    BookValue = fa.BookValue.ToString(),
                    JournalEntryNum = fa.JournalEntryNum,
                    Lifetime = fa.Lifetime,
                    WriteDownValue = fa.WriteDownValue.ToString(),
                    WriteDownRate = fa.WriteDownRate.ToString(),
                    BookValueAtYearEnd = fa.BookValueAtYearEnd.ToString(),
                });
            }

            return View(fixedAssetsViewModel);
        }


        //
        // GET: /FixedAsset/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FixedAsset/Create
        [HttpPost]
        public ActionResult Create(FixedAssetViewModel fixedAssetViewModel)
        {
            try
            {
                string currUserId = User.Identity.GetUserId();

                FixedAsset fixedAsset = new FixedAsset() 
                {
                    ApplicationUserId = currUserId,
                    NameFixedAsset = fixedAssetViewModel.Name,
                    DateFixedAsset = Convert.ToDateTime(fixedAssetViewModel.Date),
                    PurchaseValue = Convert.ToDecimal(fixedAssetViewModel.PurchaseValue),
                    BookValue = Convert.ToDecimal(fixedAssetViewModel.BookValue),
                    JournalEntryNum = fixedAssetViewModel.JournalEntryNum,
                    Lifetime = fixedAssetViewModel.Lifetime,
                    WriteDownValue = Convert.ToDecimal(fixedAssetViewModel.WriteDownValue),
                    WriteDownRate = Convert.ToDecimal(fixedAssetViewModel.WriteDownRate),
                    BookValueAtYearEnd = Convert.ToDecimal(fixedAssetViewModel.BookValueAtYearEnd)
                };

                ctx.FixedAssets.Add(fixedAsset);
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        //
        // GET: /FixedAsset/Edit/5
        public ActionResult Edit(int id)
        {
            FixedAsset fixedAsset = ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First();

            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (fixedAsset.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            FixedAssetViewModel fixedAssetViewModel = new FixedAssetViewModel()
            {
                Id = fixedAsset.IdFixedAsset,
                ApplicationUserId = fixedAsset.ApplicationUserId,
                Name = fixedAsset.NameFixedAsset,
                Date = fixedAsset.DateFixedAsset.ToShortDateString(),
                PurchaseValue = fixedAsset.PurchaseValue.ToString(),
                BookValue = fixedAsset.BookValue.ToString(),
                JournalEntryNum = fixedAsset.JournalEntryNum,
                Lifetime = fixedAsset.Lifetime,
                WriteDownValue = fixedAsset.WriteDownValue.ToString(),
                WriteDownRate = fixedAsset.WriteDownRate.ToString(),
                BookValueAtYearEnd = fixedAsset.BookValueAtYearEnd.ToString(),
            };



            return View(fixedAssetViewModel);
        }

        //
        // POST: /FixedAsset/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FixedAssetViewModel fixedAssetViewModel)
        {

            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }

            try
            {
                // TODO: Add update logic here
                

                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().DateFixedAsset = Convert.ToDateTime(fixedAssetViewModel.Date);
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().NameFixedAsset = fixedAssetViewModel.Name;
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().Lifetime = fixedAssetViewModel.Lifetime;
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().JournalEntryNum = fixedAssetViewModel.JournalEntryNum;
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().BookValue = Convert.ToDecimal(fixedAssetViewModel.BookValue);
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().PurchaseValue = Convert.ToDecimal(fixedAssetViewModel.PurchaseValue);
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().WriteDownRate = Convert.ToDecimal(fixedAssetViewModel.WriteDownRate);
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().WriteDownValue = Convert.ToDecimal(fixedAssetViewModel.WriteDownValue);
                ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First().BookValueAtYearEnd = Convert.ToDecimal(fixedAssetViewModel.BookValueAtYearEnd);

                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error", "Shared");
            }
        }

        public ActionResult Delete(int id)
        {
            FixedAsset fixedAsset = ctx.FixedAssets.Where(x => x.IdFixedAsset == id).First();
            string currUserId = User.Identity.GetUserId();
            //redirects user to the 403 page if he is trying to change data that is not his own
            if (fixedAsset.ApplicationUserId != currUserId)
            {
                throw new HttpException(403, "Forbidden");
            }
            try
            {
                // TODO: Add delete logic here
                
                ctx.FixedAssets.Remove(fixedAsset);
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
