using System.Threading.Tasks;

using Services.Common;
using Services.StateOfTexas.Models;

namespace Services.StateOfTexas.Client
{
    public interface IStateOfTexasClient
    {
        Task<ServiceResponse<NewCaseRecord>> GetLatestNewCaseCount();

        Task<ServiceResponse<int>> GetTotalCases();

        Task<ServiceResponse<DailyTestData>> GetLatestPositiveTestCount();

        Task<ServiceResponse<DailyHospitalizationRecord>> GetLastestHospitalizationCount();

        Task<ServiceResponse<DailyDeathRecord>> GetLatestDeathCount();

        Task<ServiceResponse<DailyCovidHospitalizationPctRecord>> GetLatestCovidHospitalizationPct();
    }
}