using UniversalPreferences.Algorithm;

namespace UniversalPreferences
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ExecutionManager(new ModifiedApriori());
            manager.Execute();
        }
    }
}
