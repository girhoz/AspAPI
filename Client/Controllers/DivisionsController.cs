using API.Models;
using API.MyContext;
using API.ViewModels;
using Client.Report;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        // GET: Division
        public ActionResult Index()
        {
            return View(LoadDivision()); //Tampilkan data berdasarkan fungsi loaddivision
        }

        public JsonResult LoadDivision()
        {
            IEnumerable<DivisionVM> divisions = null;
            var responseTask = client.GetAsync("Division"); //Access data from division API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<DivisionVM>>(); //Get all the data from the API
                readTask.Wait();
                divisions = readTask.Result; //Tampung setiap data didalam division
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

        public async Task<ActionResult> exportPDF()
        {
            DivReport div = new DivReport();
            var readTask = await GetDiv();
            byte[] abytes = div.PrepareReport(readTask);
            return File(abytes, "application/pdf", $"Division Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).pdf");
        }

        public async Task<List<DivisionVM>> GetDiv()
        {
            List<DivisionVM> div = new List<DivisionVM>();
            var responseTask = await client.GetAsync("Division");
            div = await responseTask.Content.ReadAsAsync<List<DivisionVM>>();
            return div;
        }


        public async Task<ActionResult> exportXLS()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:53166/api/")
            };
            //Headers Name
            var columnHeaders = new String[]
            {
                "Id",
                "Nama Divisi",
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Perbaharui"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add("All Division"); //Worksheet name
                using (var cells = worksheet.Cells[1, 1, 1, 5]) //(1,1) (1,5)
                {
                    cells.Style.Font.Bold = true;
                }

                //Add the headers
                for (var i = 0; i < columnHeaders.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnHeaders[i];
                }

                //Add values
                var j = 2;
                HttpResponseMessage response = await client.GetAsync("Division");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<DivisionVM>>();
                    foreach (var div in readTask)
                    {
                        worksheet.Cells["A" + j].Value = div.Id;
                        worksheet.Cells["B" + j].Value = div.DivisionName;
                        worksheet.Cells["C" + j].Value = div.DepartmentName;
                        worksheet.Cells["D" + j].Value = div.CreateDate;
                        worksheet.Cells["E" + j].Value = div.UpdateDate;
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Division Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).xlsx");
        }

        public async Task<ActionResult> exportCSV()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:53166/api/")
            };

            //Headers Name
            var columnHeaders = new String[]
            {
                "Id",
                "Nama Division",
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Perbaharui"
            };

            var divCSV = new StringBuilder();
            HttpResponseMessage response = await client.GetAsync("Division");
            if (response.IsSuccessStatusCode)
            {
                var readTask = await response.Content.ReadAsAsync<IList<DivisionVM>>();
                var divRecord = (from div in readTask
                                  select new object[]
                                  {
                                            div.Id,
                                            $"\"{div.DivisionName}\"", //Escaping ","
                                            $"\"{div.DepartmentName}\"",
                                            $"\"{div.CreateDate}\"",
                                            $"\"{div.UpdateDate}\""
                                  }).ToList();

                // Build the file content
                divRecord.ForEach(line =>
                {
                    divCSV.AppendLine(string.Join(",", line));
                });
            }
            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{divCSV.ToString()}");
            return File(buffer, "text/csv", $"Division Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).csv");
        }
    }
}