using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVaccineData
{
    public interface IGetVaccineDataQuery
    {
        Task<QueryResult<VaccineDataModel>> Execute();
    }
}
