using System.Threading.Tasks;

using Application.BaseModels;

namespace Application.Queries.GetToday
{
    public interface IGetTodayQuery
    {
        Task<QueryResult<Today>> Execute();
    }
}