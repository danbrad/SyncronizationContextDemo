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
    public class DeadlockController : Controller
    {
        // Deadlocks - .Result called after an await
        // https://blogs.msdn.microsoft.com/jpsanders/2017/08/28/asp-net-do-not-use-task-result-in-main-context/
        // https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
        public void Index()
        {
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Starting");
            var x = CallHttpAsyncAwait().Result;
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Finished");
        }

        // ConfigureAwait(false) makes this not deadlock but as per 
        public void AsyncSafe()
        {
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Starting");
            var x = CallHttpAAsynSafe().Result;
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Finished");
        }

        // .Result all the way down - does not deadlock
        public void Sync()
        {
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Starting");
            var x = CallHttpResult().Result;
            HttpContext.Response.Write($"{DateTime.Now.ToString("T")} - Finished");
        }

        private async Task<string>  CallHttpAsyncAwait()
        {
            using (var client = new HttpClient())
            {

                var s = await client.GetAsync("http://www.google.co.uk");
                return s.Content.ToString();

            }
        }

        private async Task<string> CallHttpAAsynSafe()
        {
            using (var client = new HttpClient())
            {

                var s = await client.GetAsync("http://www.google.co.uk").ConfigureAwait(false);
                return s.Content.ToString();
            }
        }

        private Task<string> CallHttpResult()
        {
            using (var client = new HttpClient())
            {

                var s = client.GetAsync("http://www.google.co.uk").Result;
                return Task.FromResult(s.Content.ToString());
            }
        }

    }
}
