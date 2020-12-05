using StaffandTrain.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffandTrain.Models;
using StaffandTrain.Common;

namespace StaffandTrain.Controllers
{
    [NoCache]
    [Authorize(Roles = "Admin,Recruiter")]
    public class WhiteBoardsController : Controller
    {
        Common.Common cm = new Common.Common();
        SATConn context = new SATConn();
        // GET: WhiteBoards
        public ActionResult Index()
        {
            var WhiteboardList = new List<SpGetwhiteBoard_Result>();
            try
            {
                WhiteboardList = context.SpGetwhiteBoard().ToList();
            }
            catch (Exception ex)
            {
                cm.ErrorExceptionLogingByService(ex.ToString(), "WhiteBoards" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "Index", "NA", "NA", "NA", "WEB");
            }
            return View(WhiteboardList);
        }

        public ActionResult SaveWhiteboard()
        {
            try
            {
                var Wbid = Request.Form["hdnwbid"];
                var Wbname = Request.Form["txtwbname"];
                context.SpupdatewhiteBoard(Wbname, Convert.ToInt32(Wbid));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                cm.ErrorExceptionLogingByService(ex.ToString(), "WhiteBoards" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "SaveWhiteboard", "NA", "NA", "NA", "WEB");
            }
            return (RedirectToAction("Index"));

        }

        public ActionResult WhiteBoardDetails(string wbid)
        {
            var Jobdetails = new List<SpGetJobdetailsbyWbid_Result>();
            try
            {
                if (!string.IsNullOrEmpty(wbid))
                {
                    int wbiddecrypt = Convert.ToInt32(CryptorEngine.Decrypt(cm.Code_Decrypt(Convert.ToString(wbid))));
                    Jobdetails = context.SpGetJobdetailsbyWbid(wbiddecrypt).ToList();
                    if (Jobdetails.Count() > 0)
                    {
                        ViewBag.wbname = Jobdetails.FirstOrDefault().WhiteBoardName;
                    }
                    ViewBag.wbid = wbiddecrypt;

                    if (TempData["Message"] != null)
                    {
                        ViewBag.Message = TempData["Message"];
                    }
                }

            }
            catch (Exception ex)
            {
                cm.ErrorExceptionLogingByService(ex.ToString(), "WhiteBoards" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "WhiteBoardDetails", "NA", "NA", "NA", "WEB");
            }
            return View(Jobdetails);

        }

        public ActionResult EditUpdateJobs(string jobid, string wbid)
        {
            ManageJobs objjobsdetails = new Models.ManageJobs();
            int wbiddecrypt = Convert.ToInt32(CryptorEngine.Decrypt(cm.Code_Decrypt(Convert.ToString(wbid))));
            objjobsdetails.WhiteboardID = wbiddecrypt;
            if (!string.IsNullOrEmpty(jobid))
            {
                int jobiddecrypt = Convert.ToInt32(CryptorEngine.Decrypt(cm.Code_Decrypt(Convert.ToString(jobid))));
                var jobdetails = context.spgetjobdetailbyjobid(jobiddecrypt).FirstOrDefault();
                if (jobdetails != null)
                {
                    objjobsdetails.jobid = jobdetails.jobid;
                    objjobsdetails.jobiddecypt = jobdetails.jobid;
                    objjobsdetails.jobtitle = jobdetails.jobtitle;
                    objjobsdetails.submittals = jobdetails.submittals;
                    objjobsdetails.jobdescr = jobdetails.jobdescr;

                }
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];

            }
            return View(objjobsdetails);
        }

        public ActionResult SaveJobs(ManageJobs objjobsdetails)
        {
            try
            {
                if (objjobsdetails.jobiddecypt == 0)
                {
                    context.SpcreatenewJob(objjobsdetails.jobtitle, objjobsdetails.jobdescr, objjobsdetails.submittals, objjobsdetails.WhiteboardID);
                    context.SaveChanges();
                    TempData["Message"] = "Job Created";
                }
                else
                {
                    context.Spupdatejob(objjobsdetails.jobtitle, objjobsdetails.jobdescr, objjobsdetails.submittals, objjobsdetails.jobiddecypt);
                    context.SaveChanges();
                    TempData["Message"] = "Job Updated";
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Some error occured";
                cm.ErrorExceptionLogingByService(ex.ToString(), "WhiteBoards" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "WhiteBoardDetails", "NA", "NA", "NA", "WEB");
            }

           return RedirectToAction("WhiteBoardDetails", new { @wbid = @cm.Code_Encrypt(CryptorEngine.Encrypt(objjobsdetails.WhiteboardID.ToString())) });
            //   return RedirectToAction("Index");

        }
        public ActionResult DeleteJob(string jobid, string wbid)
        {
            try
            {
                int wbiddecrypt = Convert.ToInt32(CryptorEngine.Decrypt(cm.Code_Decrypt(Convert.ToString(wbid))));
                int jobiddecrypt = Convert.ToInt32(CryptorEngine.Decrypt(cm.Code_Decrypt(Convert.ToString(jobid))));
                context.spdeletejob(jobiddecrypt);
                context.SaveChanges();
                TempData["Message"] = "Record Deleted";
                return RedirectToAction("WhiteBoardDetails", new {  @wbid = @cm.Code_Encrypt(CryptorEngine.Encrypt(wbiddecrypt.ToString())) });
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Some error occured";
                cm.ErrorExceptionLogingByService(ex.ToString(), "WhiteBoards" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "DeleteJob", "NA", "NA", "NA", "WEB");
            }


            return View();
        }
    }
}