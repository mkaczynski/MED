using UniversalPreferences.Algorithm;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ExecutionManager(new ModifiedApriori(
                new CandidatesGenerator()), new CsvDataFileManager(@"C:\Documents and Settings\Bartek\Moje dokumenty\MED\UniversalPreferences\cardatashort.txt", ",", 6)); // IoC?
            manager.Execute();
        }
    }
}
