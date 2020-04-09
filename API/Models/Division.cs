using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    [Table("Division")]
    public class Division
    {
        [Key]
        public int Id { get; set; }
        public string DivisionName { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public Division() { }

        public Division(Division division)
        {
            this.DivisionName = division.DivisionName;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
            this.Department = division.Department;
        }

        public void Update(Division division)
        {
            this.DivisionName = division.DivisionName;
            this.Department = division.Department;
            this.UpdateDate = DateTimeOffset.Now;
        }

        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }

    }
}