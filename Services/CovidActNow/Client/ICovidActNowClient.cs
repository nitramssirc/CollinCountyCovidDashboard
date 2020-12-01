using System.Threading.Tasks;

using Services.Common;
using Services.CovidActNow.Models;

namespace Services.CovidActNow.Client
{
    public interface ICovidActNowClient
    {
        Task<ServiceResponse<SingleCountySummary>> GetSingleCountySummary(string countyFipsCode);
    }
}