using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetHospitalBedCounts
{
    public interface IGetHospitalBedCountsQuery
    {
        Task<QueryResult<HospitalBedModel[]>> Execute(int numDays);
    }
}
