using StaffandTrain.Common;
using StaffandTrain.DataModel;
using StaffandTrain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace StaffandTrain.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WorkerCheckInController : ApiController
    {
        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("API response");
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult CheckInWorker(WorkerCheckInModel obj)
        {
            try
            {
                SATConn context = new SATConn();
                var worker = context.Workers.FirstOrDefault(e => e.Email == obj.Email);
                if (worker == null) throw new Exception("This worker not exist!");

                bool verifyPassword = CryptorEngine.VerifyPassword(obj.Password, worker.Password);
                if (verifyPassword == false) throw new Exception("Invalid Password!");

                context.SPInsertOrUpdateWorkerLog(0, worker.Id, worker.Name + " Worker Logged in At " + DateTime.Now, "LogIn", DateTime.Now);
                context.SaveChanges();

                if(bool.Parse(ConfigurationManager.AppSettings["EmailWorkerOnCheckIn"]))
                {
                    SendEmail mail = new SendEmail();
                    mail.SendSMTPEmail(worker.Email,  "Good Morning App - Logged In", "Hello " + worker.Name + ", you have logged in successfully At " + DateTime.Now);
                }

                var url = "https://nearshore-staffing.com/worker-check-in/?s=1&message=" + "Checked In Successfully!";
                return Redirect(url);
            }
            catch (Exception ex)
            {
                var url = "https://nearshore-staffing.com/worker-check-in/?s=0&message=" + ex.Message;
                return Redirect(url);
            }
        }
    }
    
}