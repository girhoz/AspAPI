using API.Models;
using API.ViewModels;
using Client.Report;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class DepartmentsController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:53166/api/")
        };
        // GET: Departments
        public ActionResult Index()
        {
            return View(LoadDepartment()); //Tampilkan data berdasarkan fungsi loaddepartment
        }

        public JsonResult LoadDepartment()
        {
            IEnumerable<Department> departments = null;
            var responseTask = client.GetAsync("Department"); //Access data from department API
            responseTask.Wait(); //Waits for the Task to complete execution.
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // if access success
            {
                var readTask = result.Content.ReadAsAsync<IList<Department>>(); //Get all the data from the API
                readTask.Wait();
                departments = readTask.Result; //Tampung setiap data didalam departments
            }
            else
            {
                departments = Enumerable.Empty<Department>();
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return new JsonResult { Data = departments, JsonRequestBehavior = JsonRequestBehavior.AllowGet }; //Convert kedalam hasil json dan tampilkan
        }

        public JsonResult InsertOrUpdate(Department department)
        {
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (department.Id == 0)
            {
                var result = client.PostAsync("Department", byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public async Task<JsonResult> GetById(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Department");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<IList<Department>>();
                var department = data.FirstOrDefault(D => D.Id == Id);
                var json = JsonConvert.SerializeObject(department, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return Json("internal server error");
        }

        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Department/" + Id).Result;
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public async Task<ActionResult> exportPDF()
        {
            DeptReport dept = new DeptReport();
            var readTask = await GetDept();
            byte[] abytes = dept.PrepareReport(readTask);
            return File(abytes, "application/pdf", $"Department Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).pdf");
        }

        public async Task<List<Department>> GetDept()
        {
            List<Department> dept = new List<Department>();
            var responseTask = await client.GetAsync("Department");
            dept = await responseTask.Content.ReadAsAsync<List<Department>>();
            return dept;
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
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Perbaharui"
            };

            byte[] result;

            using (var package = new ExcelPackage())
            {
                // add a new worksheet to the empty workbook
                var worksheet = package.Workbook.Worksheets.Add("All Department"); //Worksheet name
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
                HttpResponseMessage response = await client.GetAsync("Department");
                if (response.IsSuccessStatusCode)
                {
                    var readTask = await response.Content.ReadAsAsync<IList<Department>>();
                    foreach (var dept in readTask)
                    {
                        worksheet.Cells["A" + j].Value = dept.Id;
                        worksheet.Cells["B" + j].Value = dept.DepartmentName;
                        worksheet.Cells["C" + j].Value = dept.CreateDate;
                        worksheet.Cells["D" + j].Value = dept.UpdateDate;
                        j++;
                    }
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Department Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).xlsx");
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
                "Nama Department",
                "Tanggal Pembuatan",
                "Tanggal Perbaharui"
            };

            var deptCSV = new StringBuilder();
            HttpResponseMessage response = await client.GetAsync("Department");
            if (response.IsSuccessStatusCode)
            {
                var readTask = await response.Content.ReadAsAsync<IList<Department>>();
                var deptRecord = (from dept in readTask
                            select new object[]
                            {
                                            dept.Id,
                                            $"\"{dept.DepartmentName}\"", //Escaping ","
                                            $"\"{dept.CreateDate}\"",
                                            $"\"{dept.UpdateDate}\""
                            }).ToList();

                // Build the file content
                deptRecord.ForEach(line =>
                {
                    deptCSV.AppendLine(string.Join(",", line));
                });
            }
            byte[] buffer = Encoding.ASCII.GetBytes($"{string.Join(",", columnHeaders)}\r\n{deptCSV.ToString()}");
            return File(buffer, "text/csv", $"Department Report ({DateTime.Now.ToString("hh:mm:ss dd/MM/yyyy")}).csv");
        }

    }
}