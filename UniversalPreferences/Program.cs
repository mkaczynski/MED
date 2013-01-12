using UniversalPreferences.Algorithm;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ExecutionManager(new Generators(
                new CandidatesGenerator()), new CsvDataFileManager(@"..\..\cardata.txt", ",", 6, @"..\..\relations.txt")); // IoC?
            manager.Execute();
        }
    }
}
