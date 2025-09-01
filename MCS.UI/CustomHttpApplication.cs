using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Framework.Web;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Web;
using System.Diagnostics;
using System.Web.Http;

namespace MCS.UI
{
    public class CustomHttpApplication : HttpApplicationBase
    {
        public CustomHttpApplication()
        {
            this.RegisterMixedAuth();
        }

        protected static void Application_Error(Object sender, EventArgs e)
        {
        }

        public override void HttpApplicationBase_BeginRequest(Object sender, EventArgs e)
        {
            var cultureCookie = Request.Cookies["Culture"] != null && !string.IsNullOrWhiteSpace(Request.Cookies.Get("Culture").Value) ? Request.Cookies.Get("Culture").Value : "ar-JO";
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureCookie);
            culture.DateTimeFormat.ShortDatePattern = UIHelper.SystemDateFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            if (!IoC.IsInitialized)
                InitializeContainer(this);

            base.HttpApplicationBase_BeginRequest(sender, e);
        }

        //CobaltServer svr;
        public override void Application_Start(Object sender, EventArgs e)
        {
           // var port = int.Parse( ConfigurationManager.AppSettings["Port"]);
           // var docsPath = ConfigurationManager.AppSettings["DocsPath"];
           //var host = ConfigurationManager.AppSettings["Host"];
           // Process.Start("cmd");
           // svr = new CobaltServer(docsPath, host, port);
           // svr.Start();
           
            
            Logger.SetLogWriter(new LogWriterFactory().Create());

            CultureInfo cultureInfo = new CultureInfo("ar-JO");

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            // Load sipre library license
            //string key = "JwEAqs+49xYH9JRaADQBK8clme6iYLi6Ki/uAGlrgB0S/vBTdA5SNHz2DY2+pKkfSr1GE0xrTNHqVNUukethFNbkQPRze/oyJT3k9ai1zvt7qCmQP9JgQJgF5TjTaf2HheI/paUnoOTrRDfAP3c4M52gvvAbRmkh3mpXDnykBfFF1Q89wf1eQqaIODc30cvtU2dW/rvFiPhhgpTK2BBN/gprk+zB5sjC2DHZC2nKtMzOGNn3Jxwbd8XJPRKMH7nnM9cP5O8TRLpaPLs6dYfbCH4bnXc07biJ13FiNPjYP0MuiBuSmWjohxTMRmdgmPyY3hQVquRIJomME+s3jcRJ6vLV1exsEOqtUAF2JJTwrQgrrPW3O99QgQJpoXIA8MUL9HYnDuQtnaPTHu9bqGvNx/+AUcm9RfcnEWRK3lH19Tc2lZMz0NEYXEC6WR7717TLSO1NvHRgPWQAb6NeLkRIiQReR6Cmjchu9dsYJcwdMQc2dIlaUOOnUw3Rxo8yd4lm1UconeAdE87kkEaICKSXmlNRwJC2JoGoDdSLUmitkw4oIgtqZUViqhppKbZgZBUxW8TJS49BRjGZprqcteuIe5PrX8oZyttUnHsytCv3OTwExeQSXcTA0ONGvoT9iKOJRdeOpqKU8lNcHEt6UeR5rWtk1/F4kdusiJNATpx2d6l2gvF+xZnNnZ40yrQXTwBNiNCN299hAoY10/do8BJFQYfCVlVArMTqcwOdVxsMcyWLQWUmKd4nnmZSV5QwFFFsGPBedygmXFdDvgGGSp3xxPg22SPS63q3l52Qnd+JH1CVO08eoOwhMUqB5e1i0QbSrIh4pkiDiBGaj7/Wd5+k+7f0qxDToZz4cNfigFOxHU6FsDLOzQEgY7LTfBItwuFGZUjgFYdEoYGxcB+miHExGxNC9drcjtHIPaKr9/AG1RipYJtvIJfwAoHY9BUdZdpM7mKTvBOASUJCU3Ib8GZxUAprWuO3Qe1kpoxzYHPaKM5itjXsYnAdl+PdjISK0crM1IGTuo9AsF/W72WE1kig0152deHeq66q0hUOaJGifK/iUyJnyD+sf2MLBmKzFOyeTd9pIssuk4X0M3asv9ZZFwZrUeH/mGda1fOanxVYSqtEvaqFSBeSDFA4JRyTNU/WN9+EjS78DUgS8mCcUZ/8E4R8E21mi7GGc34EdEr/ZEHPrtr0csbrCYF18jNvV/r1B7H4ETLptFM5v0ALsPR1HGjJjJtB0iVO0yg0D+q0cUvqieSRHDx53PbEObwgQgOg10IfwiBwUL6hynoqRhyIZHfYMsoGtwxBTIfGLsZXChpnMMF8hcuqHbTC71cBkTJIf853zWRDQ9zS2FzU/tfEaTXoFqF91YSbEAxEi3VA1gtN5r5bBA5/d8PtwpJovPI4qpVOIyEi0DIOq1NvxtP6a4ENuvV32RaURlQkvWbK0clJ+knQ1OK3EyjeMtBmMwd6yN9ct5CTV7rOjyJ+z4nCb/k4OvG7aOmc6HY3Jy1ZUjY=";
            //Spire.License.LicenseProvider.SetLicenseKey(key);
            LicenseHelper.ModifyInMemory.ActivateMemoryPatching();


            InitializeContainer(this);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundlesConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Add(new Common.CustomViewEngines.AlternateLocationViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;
            base.Application_Start(sender, e);
        }
        protected void Application_EndRequest()
        {


            Response.Headers.Remove("Server");
        }
        public override void Application_End(Object sender, EventArgs e)
        {

           // svr.Stop();
            IoC.Reset();

            base.Application_End(sender, e);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void InitializeContainer(CustomHttpApplication self)
        {
            if (IoC.IsInitialized)
                return;

            self.CreateContainer();
        }

        private void CreateContainer()
        {
            IoC.Container = Bootstrapper.Initialize();
        }

    }
}