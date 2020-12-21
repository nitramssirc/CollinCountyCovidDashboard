using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetDeaths
{
    public interface IGetDeathsQuery
    {
        Task<QueryResult<DailyDeathModel[]>> Execute(int numDays);
    }
}
