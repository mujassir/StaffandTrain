using StaffandTrain.Common;
using StaffandTrain.DataModel;
using StaffandTrain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffandTrain.Controllers
{
    public class WorkerCheckInController : Controller
    {
        SATConn context = new SATConn();
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.message = TempData["Message"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult CheckInWorker(WorkerCheckInModel obj)
        {
            try
            {
                var worker = context.Workers.FirstOrDefault(e => e.Email == obj.Email);
                if (worker == null) throw new Exception("This worker not exist!");

                bool verifyPassword = CryptorEngine.VerifyPassword(obj.Password, worker.Password);
                if (verifyPassword == false) throw new Exception("Invalid Password!");

                context.SPInsertOrUpdateWorkerLog(0, worker.Id, worker.Name + " Worker Logged in At " + DateTime.Now, "LogIn", DateTime.Now);
                context.SaveChanges();
                SendEmail mail = new SendEmail();
                mail.SendSMTPEmail(worker.Email,  "Good Morning App - Logged In", "Hello " + worker.Name + ", you have logged in successfully At " + DateTime.Now);

                TempData["Message"] = "Checked In Successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
    
}