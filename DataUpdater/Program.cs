using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DataUpdater.DataUpdaters;
using DataUpdater.Interfaces;

namespace DataUpdater
{
    class Program
    {
        public static void Main()
        {
            var codePagesEncodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePagesEncodingProvider);

            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var updaters = GetUpdaters();
            foreach (var updater in updaters)
            {
                await updater.Execute();
            }
        }

        private static IEnumerable<IDataUpdater> GetUpdaters()
        {
            yield return new NewCasesUpdater();
            yield return new TestDataUpdater();
            yield return new FatalitiesUpdater();
            yield return new Hospitalizations_CovidHospitalizationsUpdater();
            yield return new Hospitalizations_CovidPctCapacityUpdater();
            yield return new Hospitalizations_TotalHospitalCapacityUpdater();
            yield return new Hospitalizations_TotalAvailableBedsUpdater();
            yield return new Hospitalizations_ICUBedsAvailableUpdater();
            yield return new Hospitalizations_TotalStaffedInpatientUpdater();
            yield return new Hospitalizations_CovidHospitalizationsPctUpdater();
            yield return new Hospitalizations_Covid19GeneralBedsUpdater();
            yield return new Hospitalizations_Covid19ICUUpdater();
            yield return new Hospitalizations_TotalOccupiedBedsUpdater();
        }
    }
}
