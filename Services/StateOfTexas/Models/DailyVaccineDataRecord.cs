﻿using System;
using System.Text;

namespace Services.StateOfTexas.Models
{
    public class DailyVaccineDataRecord
    {
        public DateTime Date { get; set; }
        public int VaccineDoesAllocated { get; set; }
        public int VaccineDosesAdministered { get; set; }
        public int PeopleVaccinatedWithAtLeastOneDose { get; set; }
        public int PeopleFullyVaccinated { get; set; }
        public int Population16Plus { get; set; }
        public int Population65Plus { get; set; }
        public int Phase1AHeathcareWorkers { get; set; }
        public int Phase1ALongTermCareResidents { get; set; }

        public int Phase1BAnyMedicalCondition { get; set; }

        public int EducationAndChildCarePersonnel { get; set; }

        protected bool Equals(DailyVaccineDataRecord other)
        {
            return
                VaccineDoesAllocated == other.VaccineDoesAllocated &&
                VaccineDosesAdministered == other.VaccineDosesAdministered &&
                PeopleVaccinatedWithAtLeastOneDose == other.PeopleVaccinatedWithAtLeastOneDose &&
                PeopleFullyVaccinated == other.PeopleFullyVaccinated &&
                Population16Plus == other.Population16Plus &&
                Population65Plus == other.Population65Plus &&
                Phase1AHeathcareWorkers == other.Phase1AHeathcareWorkers &&
                Phase1ALongTermCareResidents == other.Phase1ALongTermCareResidents &&
                Phase1BAnyMedicalCondition == other.Phase1BAnyMedicalCondition &&
                EducationAndChildCarePersonnel == other.EducationAndChildCarePersonnel;

        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DailyVaccineDataRecord)obj);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"    VaccineDoesAllocated:               {VaccineDoesAllocated}");
            sb.AppendLine($"    VaccineDosesAdministered:           {VaccineDosesAdministered}");
            sb.AppendLine($"    PeopleVaccinatedWithAtLeastOneDose: {PeopleVaccinatedWithAtLeastOneDose}");
            sb.AppendLine($"    PeopleFullyVaccinated:              {PeopleFullyVaccinated}");
            sb.AppendLine($"    Population16Plus:                   {Population16Plus}");
            sb.AppendLine($"    Population65Plus:                   {Population65Plus}");
            sb.AppendLine($"    Phase1AHeathcareWorkers:            {Phase1AHeathcareWorkers}");
            sb.AppendLine($"    Phase1ALongTermCareResidents:       {Phase1ALongTermCareResidents}");
            sb.AppendLine($"    Phase1BAnyMedicalCondition:         {Phase1BAnyMedicalCondition}");
            sb.AppendLine($"    EducationAndChildCarePersonnel:     {EducationAndChildCarePersonnel}");

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Date);
            hash.Add(VaccineDoesAllocated);
            hash.Add(VaccineDosesAdministered);
            hash.Add(PeopleVaccinatedWithAtLeastOneDose);
            hash.Add(PeopleFullyVaccinated);
            hash.Add(Population16Plus);
            hash.Add(Population65Plus);
            hash.Add(Phase1AHeathcareWorkers);
            hash.Add(Phase1ALongTermCareResidents);
            hash.Add(Phase1BAnyMedicalCondition);
            hash.Add(EducationAndChildCarePersonnel);
            return hash.ToHashCode();
        }
    }
}
