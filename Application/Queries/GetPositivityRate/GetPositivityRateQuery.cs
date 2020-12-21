using Application.BaseModels;

using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetPositivityRate
{
    public class GetPositivityRateQuery : IGetPositivityRateQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetPositivityRateQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetDeathsQuery Implementation

        public async Task<QueryResult<DailyPositivityRateModel[]>> Execute(int numDays)
        {
            var requestDays = numDays + 7;
            
            var response = await _stateOfTexasClient.GetLatestPositiveTestCount(requestDays);
            if (!response.WasSuccessful) { return new QueryResult<DailyPositivityRateModel[]>(response.Error); }

            var orderedData = response.Response.OrderBy(r => r.Date);
            var numDaysRangeData = orderedData.Skip(7);
            var returnData = numDaysRangeData.Select(r => new DailyPositivityRateModel(r.Date, r.PositivityRate, Get7DayAvg(r, response.Response))).ToArray();
            return new QueryResult<DailyPositivityRateModel[]>(returnData);
        }

        #endregion

        #region Private Methods

        private decimal Get7DayAvg(DailyTestData record, DailyTestData[] response)
        {
            var startDate = record.Date.AddDays(-7);
            var endDate = record.Date;
            var sevenDays = response.Where(r => r.Date > startDate && r.Date <= endDate);
            return (decimal)sevenDays.Average(r => r.PositivityRate);
        }

        #endregion

    }
}
