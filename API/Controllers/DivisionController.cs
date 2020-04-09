using API.Models;
using API.MyContext;
using API.Repository;
using API.ViewModels;
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
    public class DivisionController : ApiController
    {
        DivisionRepository division = new DivisionRepository();
        //myContext connection = new myContext();
        // GET: Department
        [HttpGet]
        //public IEnumerable<Department> Get()
        public IHttpActionResult Get()
        {
            if (division.Get() == null)
            {
                return Content(HttpStatusCode.NotFound, "Data division is empty");
            }
            return Ok(division.Get());
        }

        [ResponseType(typeof(DivisionVM))]
        public async Task<IEnumerable<DivisionVM>> GetById(int Id)
        {
            if (await division.Get(Id) == null)
            {
                return null;
            }
            return await division.Get(Id);
        }

        public IHttpActionResult Post(Division divisions)
        {
            //var getDep = connection.Departments.Where(S => S.Id == divisions.DepartmentId).FirstOrDefault();
            //if  ((getDep.IsDelete is true) && (divisions.DepartmentId == 0))
            //{
            //    return BadRequest("Department not exist");
            //}
            //else
            //{
                if ((divisions.DivisionName != null) && (divisions.DivisionName != ""))
                {
                    division.Create(divisions);
                    return Ok("Division Added Succesfully!"); //Status 200 OK
                }
                return BadRequest("Failed to Add Division");
            //}
        }

        public IHttpActionResult Put(int Id, Division divisions)
        {
            //var getDep = connection.Departments.Where(S => S.Id == divisions.DepartmentId).FirstOrDefault();
            //if ((getDep.IsDelete == true) && (divisions.DepartmentId == 0))
            //{
            //    return BadRequest("Department not exist");
            //}
            //else
            //{
                if ((divisions.DivisionName != null) && (divisions.DivisionName != ""))
                {
                    division.Update(Id, divisions);
                    return Ok("Division Updated Succesfully!"); //Status 200 OK
                }
                return BadRequest("Failed to Update Division");
            //}
        }

        public IHttpActionResult Delete(int Id)
        {
            var delete = division.Delete(Id);
            if (delete > 0)
            {
                return Ok("Division Deleted Succesfully!"); //Status 200 OK
            }
            return BadRequest("Failed to Deleted Division");
        }
    }
}
