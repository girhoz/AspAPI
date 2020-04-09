using AspAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AspAPI.Controllers
{
    public class CoronaController : Controller
    {
        readonly HttpClient client = new HttpClient();
        public CoronaController()
        {
            client.BaseAddress = new Uri("https://brmapi.azurewebsites.net/api"); // alamat acuan
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: Corona
        public ActionResult Index()
        {
            return View(LoadCorona());
        }

        public JsonResult LoadCorona()
        {
            IEnumerable<BatchesVM> countries = null;
            var responseTask = client.GetAsync("/Batches"); //halaman acuan
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<BatchesVM>>();
                readTask.Wait();
                countries = readTask.Result;
            }
            else
            {
                countries = Enumerable.Empty<BatchesVM>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(new { data = countries }, JsonRequestBehavior.AllowGet);
        }
    }
}
