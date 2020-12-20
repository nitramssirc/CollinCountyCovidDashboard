using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNewCases
{
    public interface IGetNewCasesQuery
    {
        Task<QueryResult<NewCaseModel[]>> Execute(int numDays);
    }
}
