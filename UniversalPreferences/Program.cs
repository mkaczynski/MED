using UniversalPreferences.Algorithm;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ExecutionManager(new Generators(
                new CandidatesGenerator()), new CsvDataFileManager(@"..\..\..\test_data.txt", ",", 0, @"..\..\..\test_relations.txt")); // IoC?
            manager.Execute();
        }
    }
}
