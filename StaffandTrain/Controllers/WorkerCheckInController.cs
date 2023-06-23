using StaffandTrain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffandTrain.Controllers
{
    [NoCache]
    public class WorkerCheckInController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckInWorker()
        {
            return RedirectToAction("index");
        }
    }
    
}