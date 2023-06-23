using StaffandTrain.Common;
using StaffandTrain.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffandTrain.Models;

namespace StaffandTrain.Controllers
{
    [NoCache]
    [Authorize(Roles = "Admin")]
    public class WorkerController : Controller
    {
        Common.Common cm = new Common.Common();
        SATConn context = new SATConn();

        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.message = TempData["Message"];
            }
            var WorkerList = context.Workers.ToList();
            return View(WorkerList);
        }
        public ActionResult SaveWorker()
        {
            WorkerModel objuser = new WorkerModel();
            if (Request.QueryString["id"] != null)
            {
                var Id = int.Parse(Request.QueryString["id"]);
                var worker = context.Workers.FirstOrDefault(e => e.Id == Id);

                if (worker != null)
                {
                    objuser.Id = worker.Id;
                    objuser.Name = worker.Name;
                    objuser.Email = worker.Email;
                    objuser.CheckIn = worker.CheckIn;
                }
                    

            }
            if (TempData["Message"] != null)
            {
                ViewBag.message = TempData["Message"];
            }
            return View(objuser);
        }
        [HttpPost]
        public ActionResult SaveWorkerData(Worker obj)
        {
            var worker = new Worker();
            try
            {
                worker = new Worker
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Email = obj.Email,
                    CheckIn = obj.CheckIn,
                    Password = obj.Password,
                    ModifiedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                };
                if (obj.Id == 0)
                {
                    context.Workers.Add(worker);
                    //context.SPInsertCompanies(obj.citycircle, objcomp.biztype, objcomp.name, objcomp.addr1, objcomp.addr2, objcomp.city, objcomp.state, objcomp.zip, objcomp.weburl, objcomp.phone, objcomp.priority, objcomp.target, objcomp.combinednotes, objcomp.adminnotes, objcomp.notes, objcomp.listid);
                    context.SaveChanges();
                    TempData["Message"] = "Record Saved";
                } else
                {
                    //context.SPupdateCompanies(objcomp.citycircle, objcomp.biztype, objcomp.name, objcomp.addr1, objcomp.addr2, objcomp.city, objcomp.state, objcomp.zip, objcomp.weburl, objcomp.phone, objcomp.priority, objcomp.target, objcomp.combinednotes, objcomp.adminnotes, objcomp.notes, objcomp.companyid, objcomp.listid);
                    context.SaveChanges();
                    TempData["Message"] = "Record Updated";

                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Some Error Occured";
                return View("SaveWorker", worker);
            }
        }
        public ActionResult DeleteWorker(Worker obj)
        {
            var id = Request.QueryString["id"];
            return RedirectToAction("index");
        }
    }
}