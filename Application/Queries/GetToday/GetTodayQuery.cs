using System.Threading.Tasks;

using Application.BaseModels;
using Application.Data;

using Services.Common;
using Services.CovidActNow.Client;
using Services.CovidActNow.Models;
using Services.StateOfTexas.Client;

namespace Application.Queries.GetToday
{
    public class GetTodayQuery : IGetTodayQuery
    {
        #region Dependencies

        private readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetTodayQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region Implementation of IGetTodayQuery

        public async Task<QueryResult<Today>> Execute()
        {
            var newCasesResult = await _stateOfTexasClient.GetLatestNewCaseCount();
            if (!newCasesResult.WasSuccessful) return new QueryResult<Today>(newCasesResult.Error);

            var newCasesDate = newCasesResult.Response.Date;
            var newCasesCount = newCasesResult.Response.NewCases;

            var newCasesPer100k = newCasesCount / 10.34730M;

            return new QueryResult<Today>(new Today(
                    newCasesCount,
                    newCasesPer100k,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    newCasesDate
                ));
        }


        #endregion

    }
}
