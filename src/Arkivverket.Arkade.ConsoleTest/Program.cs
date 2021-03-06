﻿using System;
using System.Collections.Generic;
using Arkivverket.Arkade.Core;
using Arkivverket.Arkade.Identify;
using Arkivverket.Arkade.Tests;
using Arkivverket.Arkade.Util;
using Autofac;
using Serilog;

namespace Arkivverket.Arkade.ConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            string archiveFileName = @"C:\Dropbox (Arkitektum AS)\Arkade5 - Testdata\Testdata\n5-aclie-in-wonderland-komplett.tar";
            string metadataFileName = @"C:\dev\src\arkade\src\Arkivverket.Arkade.Test\TestData\noark5-info.xml";
            if (args.Length != 0)
            {
                archiveFileName = args[0];
                metadataFileName = args[1];
            }

            var builder = new ContainerBuilder();
            builder.RegisterModule(new ArkadeAutofacModule());
            var container = builder.Build();

            Log.Logger = new LoggerConfiguration()
                             .MinimumLevel.Debug()
                             .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-ddTHH:mm:ss.fff} {SourceContext} [{Level}] {Message}{NewLine}{Exception}")
                             .CreateLogger();

            using (container.BeginLifetimeScope())
            {
                ArchiveExtractionReader archiveExtractionReader = container.Resolve<ArchiveExtractionReader>();

                ArchiveExtraction archiveExtraction = archiveExtractionReader.ReadFromFile(archiveFileName, metadataFileName);
                Console.WriteLine($"Reading from archive: {archiveFileName}");
                Console.WriteLine($"Uuid: {archiveExtraction.Uuid}");
                Console.WriteLine($"WorkingDirectory: {archiveExtraction.WorkingDirectory}");
                Console.WriteLine($"ArchiveType: {archiveExtraction.ArchiveType}");


                List<TestResults> testResults = new TestEngine().RunTestsOnArchive(archiveExtraction);
                foreach (TestResults results in testResults)
                {
                    Console.WriteLine($"Test: {results.TestName}, duration={results.TestDuration}, success={results.IsSuccess()}");
                }

            }

        }
    }
}
