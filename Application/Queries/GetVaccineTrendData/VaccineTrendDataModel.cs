using System;

namespace Application.Queries.GetVaccineTrendData
{
    public class VaccineTrendDataModel
    {
        public DateTime Date { get; }
        public int TotalAdministered { get; }
        public int TotalAllocated { get; }
        public int PeopleGiven1Does { get; }
        public int PeopleGivenFullDoes { get; }
        public int EligiblePopulation { get; }

        public VaccineTrendDataModel(
            DateTime date,
            int totalAdministered, 
            int totalAllocated, 
            int peopleGiven1Does, 
            int peopleGivenFullDoes, 
            int eligiblePopulation)
        {
            Date = date;
            TotalAdministered = totalAdministered;
            TotalAllocated = totalAllocated;
            PeopleGiven1Does = peopleGiven1Does;
            PeopleGivenFullDoes = peopleGivenFullDoes;
            EligiblePopulation = eligiblePopulation;
        }
    }
}
