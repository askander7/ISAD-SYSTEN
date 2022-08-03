using ISAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISAD.Controllers
{
    [AuthorizedUserAttribute]
    public class UsersController : Controller
    {
        private readonly ISADDBEntities db = new ISADDBEntities();
        [HttpGet]
        public ActionResult PrimaryData()
        {
            int uid = Convert.ToInt32(Session["uid"]);
            var userDb = db.Users.FirstOrDefault(a => a.ID == uid);
            var user = new PrimaryData { 
            ID=userDb.ID,
            FullName=userDb.FullName,
            UserIdentity=userDb.UserIdentity,
            MaritalStatus=userDb.MaritalStatus,
            Birthdate=userDb.Birthdate,
            DivorceDate=userDb.DivorceDate,
            DivorceReason=userDb.DivorceReason,
            HasDivorceFamily=userDb.HasDivorceFamily,
            HasDivorceReason=userDb.HasDivorceReason,
            IdExpDate=userDb.IdExpDate
            };
            return View(user);
        }


        [HttpPost]
        public ActionResult PrimaryData(PrimaryData user)
        {
            try
            {
            var userDb = db.Users.FirstOrDefault(a => a.ID == user.ID);
            userDb.FullName = user.FullName;
            userDb.UserIdentity = user.UserIdentity;
            userDb.MaritalStatus = user.MaritalStatus;
            userDb.IdExpDate = user.IdExpDate;
            userDb.Birthdate = user.Birthdate;
            userDb.DivorceDate = user.DivorceDate;
            userDb.HasDivorceReason = user.HasDivorceReason;
            userDb.HasDivorceFamily = user.HasDivorceFamily;
            userDb.DivorceReason = user.DivorceReason;
            db.Entry(userDb).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View(user);
        }


    }
}