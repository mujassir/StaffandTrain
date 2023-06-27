using StaffandTrain.App_Start;
using StaffandTrain.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace StaffandTrain
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Timer timer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional }
            );
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;

            
            StartTimer();
        }
        protected void Application_End(object sender, EventArgs e)
        {
            timer?.Dispose();
        }

        private void StartTimer()
        {
            int minute = int.Parse(ConfigurationManager.AppSettings["EmailWorkerSchedule"]);
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(minute));
        }

        private void TimerCallback(object state)
        {
            SendEmail email = new SendEmail();
            email.ScheduleWorkerEmail();
        }
    }
}
