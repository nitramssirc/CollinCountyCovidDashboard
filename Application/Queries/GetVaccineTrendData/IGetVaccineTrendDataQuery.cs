using Application.BaseModels;

using System.Threading.Tasks;

namespace Application.Queries.GetVaccineTrendData
{
    public interface IGetVaccineTrendDataQuery
    {
        Task<QueryResult<VaccineTrendDataModel[]>> Execute(int numDays);
    }
}
