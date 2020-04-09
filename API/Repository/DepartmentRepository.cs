using API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;

namespace API.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
  
        public int Create(Department department)
        {
            var spName = "SP_InsertDepartment";
            parameters.Add("@Name", department.DepartmentName);
            var create = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            var spName = "SP_DeleteDepartment";
            parameters.Add("@Id", Id);
            var delete = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return delete;
        }

        public IEnumerable<Department> Get()
        {
            var spName = "SP_ViewDepartment";
            var view = connection.Query<Department>(spName, commandType: CommandType.StoredProcedure);
            return view;
        }

        public async Task<IEnumerable<Department>> Get(int Id)
        {
            var spName = "SP_GetDepartmentById";
            parameters.Add("@Id", Id);
            var getDep = await connection.QueryAsync<Department>(spName, parameters, commandType: CommandType.StoredProcedure);
            return getDep;
        }

        public int Update(int Id, Department department)
        {
            var spName = "SP_UpdateDepartment";
            parameters.Add("@Id", Id);
            parameters.Add("@Name", department.DepartmentName);
            var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}