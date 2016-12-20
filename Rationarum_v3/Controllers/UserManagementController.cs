using Rationarum_v3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rationarum_v3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //
        //Lists all the users, except the current one and displays name, email and role
        // GET: /UserManagement/
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();//mora ovako
            List<ApplicationUser> user = ctx.Users.Where(x => x.Id != userId).ToList();
            List<UserManagementViewModel> userRoleList = new List<UserManagementViewModel>();
            //ovdje dolazi do problema ako nije sve lijepo pobrisano
            foreach(ApplicationUser u in user)
            {
                userRoleList.Add(new UserManagementViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Roles.First().Role,
                    AssociationName = u.AssociationName,
                    Adress = u.Adress,
                    OIB = u.OIB
                });
            }

            return View(userRoleList);
        }


        //
        // GET: /UserManagement/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.RoleList = ctx.Roles.Select(n => n.Name).ToList();
            //ViewBag.RoleList = new List<string>() { "Admin", "AssociationUser", "BlockedAssociationUser" };
            var userManagement = new UserManagementViewModel
                {
                    Id = ctx.Users.Where(u => u.Id == id).First().Id,
                    UserName = ctx.Users.Where(u => u.Id == id).First().UserName,
                    Email = ctx.Users.Where(u => u.Id == id).First().Email,
                    AssociationName = ctx.Users.Where(u => u.Id == id).First().AssociationName,
                    Adress = ctx.Users.Where(u => u.Id == id).First().Adress,
                    OIB = ctx.Users.Where(u => u.Id == id).First().OIB,
                    Role = ctx.Users.Where(u => u.Id == id).First().Roles.First().Role
                };
            
            return View(userManagement);
        }

        //
        // POST: /UserManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, UserManagementViewModel userManagement)
        {
            try
            {
                // TODO: Add update logic here
                ctx.Users.Where(u => u.Id == id).First().UserName = userManagement.UserName;
                ctx.Users.Where(u => u.Id == id).First().Email = userManagement.Email;
                ctx.Users.Where(u => u.Id == id).First().OIB = userManagement.OIB;
                ctx.Users.Where(u => u.Id == id).First().AssociationName = userManagement.AssociationName;
                ctx.Users.Where(u => u.Id == id).First().Adress = userManagement.Adress;

                userManager.RemoveFromRole(id, ctx.Users.Where(u => u.Id == id).First().Roles.First().Role.Name);
                userManager.AddToRole(id, userManagement.Role.Name);

                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /UserManagement/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                //TODO: Add delete logic here
                ctx.Expenditures.RemoveRange(ctx.Expenditures.Where(e => e.ApplicationUserId == id).ToList());
                ctx.Receipts.RemoveRange(ctx.Receipts.Where(e => e.ApplicationUserId == id).ToList());
                ctx.IngoingInvoices.RemoveRange(ctx.IngoingInvoices.Where(e => e.ApplicationUserId == id).ToList());
                ctx.OutgoingInvoices.RemoveRange(ctx.OutgoingInvoices.Where(e => e.ApplicationUserId == id).ToList());
                userManager.RemoveFromRole(id, userManager.GetRoles(id).First());
                ctx.Users.Remove(ctx.Users.Where(u => u.Id == id).First());
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ResetPassword(string id)
        {
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());

            userManager.RemovePassword(id);

            userManager.AddPassword(id, "rationarum0");

            return RedirectToAction("Index");
        }
    }
}
