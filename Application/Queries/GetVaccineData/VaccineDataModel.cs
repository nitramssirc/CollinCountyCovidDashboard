using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetVaccineData
{
    public class VaccineDataModel
    {
        public DateTime UpdateDate { get; }
        public int NumAdministeredToday { get; }
        public int TotalAdministered { get; }
        public int AllocatedToday { get; }
        public int TotalAllocated { get; }
        public int PeopleGiven1Does { get; }
        public decimal PctOfEligibleGiven1Does { get; }
        public int PeopleGivenFullDoes { get; }
        public decimal PctOfEligibleGivenFullDoes { get; }

        public VaccineDataModel(
            DateTime updateDate,
            int numAdministeredToday, 
            int totalAdministered, 
            int allocatedToday, 
            int totalAllocated, 
            int peopleGiven1Does, 
            decimal pctOfEligibleGiven1Does, 
            int peopleGivenFullDoes, 
            decimal pctOfEligibleGivenFullDoes)
        {
            UpdateDate = updateDate;
            NumAdministeredToday = numAdministeredToday;
            TotalAdministered = totalAdministered;
            AllocatedToday = allocatedToday;
            TotalAllocated = totalAllocated;
            PeopleGiven1Does = peopleGiven1Does;
            PctOfEligibleGiven1Does = pctOfEligibleGiven1Does;
            PeopleGivenFullDoes = peopleGivenFullDoes;
            PctOfEligibleGivenFullDoes = pctOfEligibleGivenFullDoes;
        }
    }
}
