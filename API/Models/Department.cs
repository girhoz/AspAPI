﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }

        public Department() { }

        public Department(Department department)
        {
            this.DepartmentName = department.DepartmentName;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
        }

        public void Update(Department department)
        {
            this.DepartmentName = department.DepartmentName;
            this.UpdateDate = DateTimeOffset.Now;
        }

        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }

    }
}