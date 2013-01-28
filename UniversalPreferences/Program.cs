using System;
using System.IO;
using System.Threading;
using UniversalPreferences.Algorithm;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    class Program
    {
        private const string filePath = "memUsage.txt";
        private long counter = 0;

        public Program()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);                
            }
        }

        static void Main(string[] args)
        {
            try
            {
                var parser = new ArgumentParser(args);
                var arguments = parser.ParseArguments();

                var p = new Program();
                using (new Timer(x => p.WriteMemoryUsage(), null, 0, 3000))
                {
                    var dataManager = new CsvDataFileManager(
                        arguments.DataFilePath,
                        arguments.Delimiter,
                        arguments.ClassIndex,
                        arguments.RelationsFilePath,
                        arguments.RelationKind,
                        arguments.PreferenceMatrix);

                    var manager = new ExecutionManager(
                        new Generators(
                            arguments.HashTreePageSize,
                            arguments.HashTreeFirstNumber,
                            arguments.WriteIterationResults,
                            arguments.Method,
                            new CandidatesGenerator(arguments.HashTreePageSize, arguments.HashTreeFirstNumber), 
                            new ResultConverter(dataManager)), dataManager);

                    manager.Execute();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void WriteMemoryUsage()
        {
            var memUsage = GC.GetTotalMemory(false);
            using (var streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(string.Format("{0} {1}", ++counter, memUsage));
            }
        }
    }
}
