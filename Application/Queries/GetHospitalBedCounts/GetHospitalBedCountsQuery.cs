

using Application.BaseModels;

using Services.StateOfTexas.Client;

using System.Linq;
using System.Threading.Tasks;

namespace Application.Queries.GetHospitalBedCounts
{
    public class GetHospitalBedCountsQuery : IGetHospitalBedCountsQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetHospitalBedCountsQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetDeathsQuery Implementation

        public async Task<QueryResult<HospitalBedModel[]>> Execute(int numDays)
        {           
            var response = await _stateOfTexasClient.GetHospitalBedRecords(numDays);
            if (!response.WasSuccessful) { return new QueryResult<HospitalBedModel[]>(response.Error); }

            var returnData = response.Response.Select(r => new HospitalBedModel(r.Date, r.Capacity, r.TotalOccupied, r.CovidOccupied)).ToArray();
            return new QueryResult<HospitalBedModel[]>(returnData);
        }

        #endregion

    }
}
