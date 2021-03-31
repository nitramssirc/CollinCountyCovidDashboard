using Application.BaseModels;

using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Queries.GetVaccineTrendData
{
    public class GetVaccineTrendDataQuery : IGetVaccineTrendDataQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetVaccineTrendDataQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetVaccineTrendDataQuery Implementation

        public async Task<QueryResult<VaccineTrendDataModel[]>> Execute(int numDays)
        {
            var vaccineDataRecords = await _stateOfTexasClient.GetVaccineRecords(numDays);
            if (!vaccineDataRecords.WasSuccessful) return new QueryResult<VaccineTrendDataModel[]>(vaccineDataRecords.Error);

            var returnModel = vaccineDataRecords.Response.Select(d=>ConstructModel(d)).OrderBy(d=>d.Date).ToArray();
            return new QueryResult<VaccineTrendDataModel[]>(returnModel);
        }

        #endregion

        #region Private Methods

        private VaccineTrendDataModel ConstructModel(DailyVaccineDataRecord record)
        {
            int eligiblePopulation = GetEligiblePopulation(record);
            return new VaccineTrendDataModel(
                record.Date,
                record.VaccineDosesAdministered,
                record.VaccineDoesAllocated,
                record.PeopleVaccinatedWithAtLeastOneDose,
                record.PeopleFullyVaccinated,
                eligiblePopulation
            );

        }

        private int GetEligiblePopulation(DailyVaccineDataRecord record)
        {
            if (record.Date < new DateTime(2021, 3, 29))
            {
                return record.Phase1AHeathcareWorkers +
                record.Phase1ALongTermCareResidents +
                record.Phase1BAnyMedicalCondition +
                record.EducationAndChildCarePersonnel;
            }
            else
            {
                return record.Population16Plus;
            }
        }


        #endregion

    }
}
