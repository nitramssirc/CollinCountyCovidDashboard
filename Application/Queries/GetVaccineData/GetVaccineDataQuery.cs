using Application.BaseModels;

using Services.Common;
using Services.StateOfTexas.Client;
using Services.StateOfTexas.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVaccineData
{
    public class GetVaccineDataQuery : IGetVaccineDataQuery
    {
        #region Dependencies

        readonly IStateOfTexasClient _stateOfTexasClient;

        #endregion

        #region Constructor

        public GetVaccineDataQuery(IStateOfTexasClient stateOfTexasClient)
        {
            _stateOfTexasClient = stateOfTexasClient;
        }

        #endregion

        #region IGetVaccineDataQuery Implementation

        public async Task<QueryResult<VaccineDataModel>> Execute()
        {
            var numDays = 2;
            var vaccineDataRecords = await _stateOfTexasClient.GetVaccineRecords(numDays);
            if (!vaccineDataRecords.WasSuccessful) return new QueryResult<VaccineDataModel>(vaccineDataRecords.Error);

            var returnModel = ConstructModel(vaccineDataRecords.Response);
            return new QueryResult<VaccineDataModel>(returnModel);
        }

        #endregion

        #region Private Methods

        private VaccineDataModel ConstructModel(DailyVaccineDataRecord[] vaccineDataRecords)
        {
            var today = vaccineDataRecords[0];
            var yesterday = vaccineDataRecords[1];

            decimal eligiblePopulation = GetEligiblePopulation(today);
            return new VaccineDataModel(
                today.Date,
                today.VaccineDosesAdministered - yesterday.VaccineDosesAdministered,
                today.VaccineDosesAdministered,
                today.VaccineDoesAllocated - yesterday.VaccineDoesAllocated,
                today.VaccineDoesAllocated,
                today.PeopleVaccinatedWithAtLeastOneDose,
                today.PeopleVaccinatedWithAtLeastOneDose / eligiblePopulation,
                today.PeopleFullyVaccinated,
                today.PeopleFullyVaccinated / eligiblePopulation
            );

        }

        private int GetEligiblePopulation(DailyVaccineDataRecord record)
        {
            if(record.Date < new DateTime(2021, 3, 29))
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
