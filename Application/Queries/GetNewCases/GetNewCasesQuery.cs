using Application.BaseModels;

using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNewCases
{
    public class GetNewCasesQuery : IGetNewCasesQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetNewCasesQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetNewCasesQuery Implementation

        public async Task<QueryResult<NewCaseModel[]>> Execute(int numDays)
        {
            var requestDays = numDays + 7;
            
            var response = await _stateOfTexasClient.GetLatestNewCaseRecords(requestDays);
            if (!response.WasSuccessful) { return new QueryResult<NewCaseModel[]>(response.Error); }

            var orderedData = response.Response.OrderBy(r => r.Date);
            var numDaysRangeData = orderedData.Skip(7);
            var returnData = numDaysRangeData.Select(r => new NewCaseModel(r.Date, r.NewCases, Get7DayAvg(r, response.Response))).ToArray();
            return new QueryResult<NewCaseModel[]>(returnData);
        }

        #endregion

        #region Private Methods

        private decimal Get7DayAvg(NewCaseRecord record, NewCaseRecord[] response)
        {
            var startDate = record.Date.AddDays(-7);
            var endDate = record.Date;
            var sevenDays = response.Where(r => r.Date > startDate && r.Date <= endDate);
            return (decimal)sevenDays.Average(r => r.NewCases);
        }

        #endregion

    }
}
