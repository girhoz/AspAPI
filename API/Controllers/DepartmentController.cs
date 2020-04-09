using API.Models;
using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class DepartmentController : ApiController
    {
        DepartmentRepository department = new DepartmentRepository();
        // GET: Department
        [HttpGet]
        //public IEnumerable<Department> Get()
        public IHttpActionResult Get()
        {
            if (department.Get() == null)
            {
                return Content(HttpStatusCode.NotFound, "Data department is empty");
            }
            return Ok(department.Get());
        }

        [ResponseType(typeof(Department))]
        public async Task<IEnumerable<Department>> GetById(int Id)
        {
            if (await department.Get(Id) == null)
            {
                return null;
            }
            return await department.Get(Id);
        }

        public IHttpActionResult Post(Department departments)
        {
            if ((departments.DepartmentName != null) && (departments.DepartmentName != ""))
            {
                department.Create(departments);
                return Ok("Department Added Succesfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Add Department");
        }

        public IHttpActionResult Put(int Id, Department departments)
        {
            if ((departments.DepartmentName != null) && (departments.DepartmentName != ""))
            {
                department.Update(Id, departments);
                return Ok("Department Updated Succesfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Update Department");
        }

        public IHttpActionResult Delete(int Id)
        {
            var delete = department.Delete(Id);
            if (delete > 0)
            {
                return Ok("Department Deleted Succesfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Deleted Department");
        }

    }
}
