using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetICUBedCounts
{
    public interface IGetICUBedCountsQuery
    {
        Task<QueryResult<ICUBedModel[]>> Execute(int numDays);
    }
}
