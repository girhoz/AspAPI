﻿using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interface
{
    interface IDepartmentRepository
    {
        IEnumerable<Department> Get();
        Task<IEnumerable<Department>> Get(int Id);

        int Create(Department department);
        int Update(int Id, Department department);
        int Delete(int Id);
    }
}