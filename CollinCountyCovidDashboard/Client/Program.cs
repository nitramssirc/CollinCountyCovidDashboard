
using Application.Queries.Get7DayAvg;
using Application.Queries.GetDeaths;
using Application.Queries.GetHospitalBedCounts;
using Application.Queries.GetICUBedCounts;
using Application.Queries.GetNewCases;
using Application.Queries.GetPositivityRate;
using Application.Queries.GetToday;
using Application.Queries.GetVaccineData;
using Application.Queries.GetVaccineTrendData;

using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Services.StateOfTexas.Client;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CollinCountyCovidDashboard.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(typeof(IStateOfTexasClient), typeof(StateOfTexasClient));
            builder.Services.AddScoped(typeof(IGetTodayQuery), typeof(GetTodayQuery));
            builder.Services.AddScoped(typeof(IGet7DayAvgQuery), typeof(Get7DayAvgQuery));
            builder.Services.AddScoped(typeof(IGetNewCasesQuery), typeof(GetNewCasesQuery));
            builder.Services.AddScoped(typeof(IGetDeathsQuery), typeof(GetDeathsQuery));
            builder.Services.AddScoped(typeof(IGetPositivityRateQuery), typeof(GetPositivityRateQuery));
            builder.Services.AddScoped(typeof(IGetHospitalBedCountsQuery), typeof(GetHospitalBedCountsQuery));
            builder.Services.AddScoped(typeof(IGetICUBedCountsQuery), typeof(GetICUBedCountsQuery));
            builder.Services.AddScoped(typeof(IGetVaccineDataQuery), typeof(GetVaccineDataQuery));
            builder.Services.AddScoped(typeof(IGetVaccineTrendDataQuery), typeof(GetVaccineTrendDataQuery));

            var codePagesEncodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePagesEncodingProvider);

            await builder.Build().RunAsync();
        }
    }
}
