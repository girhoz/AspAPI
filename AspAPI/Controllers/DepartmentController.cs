using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AspAPI.Controllers
{
    public class DepartmentController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:53166/api/")
        };

        public ActionResult Index()
        {
            return View(LoadDepartment());
        }

        public JsonResult LoadDepartment()
        {
            IEnumerable<Department> departments = null;
            var responseTask = client.GetAsync("/Department"); //halaman acuan
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>();
                readTask.Wait();
                departments = readTask.Result;
            }
            else
            {
                departments = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return new JsonResult { Data = departments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}