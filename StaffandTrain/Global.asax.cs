using StaffandTrain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StaffandTrain
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Timer timer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
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
            timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        }

        private void TimerCallback(object state)
        {
            SendEmail email = new SendEmail();
            email.ScheduleWorkerEmail();
        }
    }
}
