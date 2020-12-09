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
    [Authorize(Roles = "Admin")]
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
                var companySearchQuery = "EXEC DBO.SearchCompany @SearchQuery = '" + searchQuery + "'";
                var contactSearchQuery = "EXEC DBO.SearchContact @SearchQuery = '" + searchQuery + "'";
                var companies = context.Database.SqlQuery<Company>(companySearchQuery);
                var contacts = context.Database.SqlQuery<Contact>(contactSearchQuery);
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
