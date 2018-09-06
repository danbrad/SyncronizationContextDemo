using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SyncronizationContextDemo.Controllers
{
    public class ContextController : Controller
    {
        // Using the ambient HttpContext.Current after ConfigureAwait(false) fails
        // https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
        public async Task Ambient()
        {
            System.Web.HttpContext.Current.Response.Write($"{DateTime.Now.ToString("T")} - Starting");

            using (var client = new HttpClient())
            {

                await client.GetAsync("http://www.gogole.co.uk").ConfigureAwait(false);

            }

            System.Web.HttpContext.Current.Response.Write($"{DateTime.Now.ToString("T")} - Finished");
            
        }


        // Using the base controller instance HttpContext after ConfigureAwait(false) does not fail
        public async Task Instance()
        {
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Starting");

            using (var client = new HttpClient())
            {

                await client.GetAsync("http://www.gogole.co.uk").ConfigureAwait(false);

            }

            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Finished");

        }

    }
}
