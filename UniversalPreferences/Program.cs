using UniversalPreferences.Algorithm;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ExecutionManager(new Generators(
                new CandidatesGenerator()), new SimpleData()); // IoC?
            manager.Execute();
        }
    }
}
