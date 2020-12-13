
using Application.Queries.Get7DayAvg;
using Application.Queries.GetToday;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Services.CovidActNow.Client;
using Services.StateOfTexas.Client;

using System;
using System.Collections.Generic;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(typeof(IStateOfTexasClient), typeof(StateOfTexasClient));
            builder.Services.AddScoped(typeof(IGetTodayQuery), typeof(GetTodayQuery));
            builder.Services.AddScoped(typeof(IGet7DayAvgQuery), typeof(Get7DayAvgQuery));

            var codePagesEncodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePagesEncodingProvider);

            await builder.Build().RunAsync();
        }
    }
}
