using Application.BaseModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetPositivityRate
{
    public interface IGetPositivityRateQuery
    {
        Task<QueryResult<DailyPositivityRateModel[]>> Execute(int numDays);
    }
}
