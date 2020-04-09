using API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using API.ViewModels;

namespace API.Repository
{
    public class DivisionRepository : IDivisionRepository
    {
        DynamicParameters parameters = new DynamicParameters();
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        public int Create(Division division)
        {
            var spName = "SP_InsertDivision";
            parameters.Add("@Name", division.DivisionName);
            parameters.Add("@DepId", division.DepartmentId);
            var create = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            var spName = "SP_DeleteDivision";
            parameters.Add("@Id", Id);
            var delete = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return delete;
        }

        public IEnumerable<DivisionVM> Get()
        {
            var spName = "SP_ViewDivision";
            var view = connection.Query<DivisionVM>(spName, commandType: CommandType.StoredProcedure);
            return view;
        }

        public async Task<IEnumerable<DivisionVM>> Get(int Id)
        {
            var spName = "SP_GetDivisionById";
            parameters.Add("@Id", Id);
            var getDiv = await connection.QueryAsync<DivisionVM>(spName, parameters, commandType: CommandType.StoredProcedure);
            return getDiv;
        }

        public int Update(int Id, Division division)
        {
            var spName = "SP_UpdateDivision";
            parameters.Add("@Id", Id);
            parameters.Add("@Name", division.DivisionName);
            parameters.Add("@DepId", division.DepartmentId);
            var update = connection.Execute(spName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}