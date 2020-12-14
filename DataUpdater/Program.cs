﻿using System;
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
        }
    }
}
