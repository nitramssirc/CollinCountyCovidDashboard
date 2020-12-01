using System.Threading.Tasks;

using Services.Common;
using Services.StateOfTexas.Models;

namespace Services.StateOfTexas.Client
{
    public interface IStateOfTexasClient
    {
        Task<ServiceResponse<NewCaseRecord>> GetLatestNewCaseCount();

        Task<ServiceResponse<int>> GetTotalCases();
    }
}