using System;

namespace Services.Common
{
    public class ServiceResponse<T>
    {
        public bool WasSuccessful { get; private set; }

        public string Error { get; private set; }

        public T Response { get; private set; }

        public ServiceResponse(T response)
        {
            WasSuccessful = true;
            Error = null;
            Response = response;
        }

        public ServiceResponse(string error)
        {
            WasSuccessful = false;
            Error = error;
            Response = default;
        }

        public ServiceResponse(string error, Exception ex)
        {
            WasSuccessful = false;
#if DEBUG
            Error = $"{error}.  Ex:{ex}";
#else
            Error = error;
#endif
            Response = default;
        }
    }
}
