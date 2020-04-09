using API.Models;
using API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interface
{
    interface IDivisionRepository
    {
        IEnumerable<DivisionVM> Get();
        Task<IEnumerable<DivisionVM>> Get(int Id);

        int Create(Division division);
        int Update(int Id, Division division);
        int Delete(int Id);
    }
}
