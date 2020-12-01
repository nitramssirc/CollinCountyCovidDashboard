using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BaseModels
{
    public class QueryResult<T>
    {
        public bool WasSuccessful { get; private set; }

        public string Error { get; private set; }

        public T Result { get; private set; }

        public QueryResult(T result)
        {
            WasSuccessful = true;
            Error = null;
            Result = result;
        }

        public QueryResult(string error)
        {
            WasSuccessful = false;
            Error = error;
            Result = default;
        }
    }
}
