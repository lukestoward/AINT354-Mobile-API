using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AINT354_Mobile_API.BusinessLogic;
using AINT354_Mobile_API.Migrations;

namespace AINT354_Mobile_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult About()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();

            //AndroidGCMPushNotification.SendNotification(
            //   "APA91bHOpo_ODjM6TsQpcWB9PNb_HfiV-6EFQhQrKWDcS1tPaKKnt-cyqy50xRPhuW68tbTNG5JeLK7bUOq8lA_fRK_JUd9yjbaymTEI9b6IrHpnetvAiww_u8FE22ZW2SFzI5i2oDJq",
            //    "Fuck yeah it worked!");

            return View("Index");
        }
    }
}
