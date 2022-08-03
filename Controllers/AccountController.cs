using ISAD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISAD.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISADDBEntities db = new ISADDBEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new UserMeta());
        }
        [HttpPost]
        public ActionResult Register(UserMeta user)
        {
            try
            {

            
            if(!user.ApprovePrivacypolicy)
            {
                TempData["msg"] = "Sorry, you should approve privacy policy";
                return View();
            }
            user.ID = db.Users.Any() ? db.Users.Max(a => a.ID) + 1 : 1;

            db.Users.Add(new Models.User {
            ID=user.ID,
            FullName=user.FullName,
            Username=user.Username,
            Password=user.Password,
            City=user.City,
            MaritalStatus=user.MaritalStatus,
            Mobile=user.Mobile,
            UserIdentity=user.UserIdentity
            });
            int result = db.SaveChanges();
            if(result==0)
            {
            TempData["msg"] = "Register faild, try again!";
            return View();
            }
            return RedirectToAction(nameof(Login));
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder msg = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    msg.Append($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        msg.Append($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                TempData["msg"] = msg;
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error!";
            }
           
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserMeta user)
        {
            try
            {
            if(db.Users.Any(a=>a.Username==user.Username && a.Password==user.Password))
            {
                Session["uid"] = db.Users.FirstOrDefault(a => a.Username == user.Username && a.Password == user.Password).ID;
                HttpCookie version = new HttpCookie("Isad-version", System.Configuration.ConfigurationManager.AppSettings["Version"]);
                HttpCookie username = new HttpCookie("Isad-username", user.Username);
                HttpCookie password = new HttpCookie("Isad-password", user.Password);
                version.Expires = DateTime.Now.AddYears(1);
                username.Expires = DateTime.Now.AddYears(1);
                password.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(version);
                Response.Cookies.Add(username);
                Response.Cookies.Add(password);
                return RedirectToAction("Index", "Home");
            }
            TempData["msg"] = "Username or Password is wrong!";
            return View();
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction(nameof(Login));
        }
    }
}