using System.Collections.Generic;
using System.Threading.Tasks;

using Services.Common;
using Services.StateOfTexas.Models;

namespace Services.StateOfTexas.Client
{
    public interface IStateOfTexasClient
    {
        Task<ServiceResponse<NewCaseRecord[]>> GetLatestNewCaseRecords(int numDays);

        Task<ServiceResponse<int>> GetTotalCases();

        Task<ServiceResponse<DailyTestData[]>> GetLatestPositiveTestCount(int numDays);

        Task<ServiceResponse<DailyHospitalizationRecord[]>> GetLastestHospitalizationCount(int numDays);

        Task<ServiceResponse<DailyDeathRecord[]>> GetLatestDeathCount(int numDays);

        Task<ServiceResponse<DailyHospitalBedRecord[]>> GetHospitalBedRecords(int numDays);
    
        Task<ServiceResponse<DailyHospitalBedRecord[]>> GetICUBedRecords(int numDays);

        Task<ServiceResponse<DailyVaccineDataRecord[]>> GetVaccineRecords(int numDays);
    }
}