using API.Models;
using API.MyContext;
using API.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class DivisionsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:53166/api/")
        };
        // GET: Departments
        public ActionResult Index()
        {
            return View(LoadDivision()); //Tampilkan data berdasarkan fungsi loaddepartment
        }

        public JsonResult LoadDivision()
        {
            IEnumerable<DivisionVM> divisions = null;
            var responseTask = client.GetAsync("Division"); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisionVM>>(); //Get all the data from the API
                readTask.Wait();
                divisions = readTask.Result; //Tampung setiap data didalam departments
            }
            else
            {
                divisions = Enumerable.Empty<DivisionVM>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return new JsonResult { Data = divisions, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; //Convert kedalam hasil json dan tampilkan
        }

        public JsonResult InsertOrUpdate(Division division)
        {
            var myContent = JsonConvert.SerializeObject(division);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (division.Id == 0)
            {
                var result = client.PostAsync("Division", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var result = client.PutAsync("Division/" + division.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Division");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<DivisionVM>>();
                var division = data.FirstOrDefault(D => D.Id == Id);
                var json = JsonConvert.SerializeObject(division, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("internal server error");
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Division/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}