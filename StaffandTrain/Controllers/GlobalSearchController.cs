using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using StaffandTrain.Common;
using StaffandTrain.DataModel;
using StaffandTrain.Models;

namespace StaffandTrain.Controllers
{
    [NoCache]
    [Authorize(Roles = "Admin,Recruiter")]
    public class GlobalSearchController : Controller
    {
        // GET: GlobalSearch
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSearchResults(string searchQuery)
        {
            SATConn context = new SATConn();
            try
            {
                var companies = context.SearchCompany(searchQuery);
                var contacts = context.SearchContact(searchQuery);
                var result = new
                {
                    success = true,
                    companies = companies,
                    contacts = contacts
                };
                return Json(result);
            }
            catch (Exception ex)
            {

                var result = new
                {
                    success = false,
                    error = ex
                };
                return Json(result);
            }

        }


    }
}
