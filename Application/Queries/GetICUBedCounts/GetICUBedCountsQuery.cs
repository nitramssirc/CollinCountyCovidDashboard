

using Application.BaseModels;

using Services.StateOfTexas.Client;

using System.Linq;
using System.Threading.Tasks;

namespace Application.Queries.GetICUBedCounts
{
    public class GetICUBedCountsQuery : IGetICUBedCountsQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetICUBedCountsQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetDeathsQuery Implementation

        public async Task<QueryResult<ICUBedModel[]>> Execute(int numDays)
        {           
            var response = await _stateOfTexasClient.GetICUBedRecords(numDays);
            if (!response.WasSuccessful) { return new QueryResult<ICUBedModel[]>(response.Error); }

            var returnData = response.Response
                .Select(r => new ICUBedModel(r.Date, r.Capacity, r.TotalOccupied, r.CovidOccupied))
                .OrderBy(r=> r.Date)
                .ToArray();
            return new QueryResult<ICUBedModel[]>(returnData);
        }

        #endregion

    }
}
