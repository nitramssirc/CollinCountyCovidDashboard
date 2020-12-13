using System.Threading.Tasks;

using Application.BaseModels;

namespace Application.Queries.Get7DayAvg
{
    public interface IGet7DayAvgQuery
    {
        Task<QueryResult<SevenDayAvg>> Execute();
    }
}