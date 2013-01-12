using System.Collections.Generic;
using System.Linq;

namespace UniversalPreferences.DAL
{
    class Preference
    {
        public string Name { get; private set; }

        public IList<Preference> Betters { get; private set; }

        public Preference(string name)
        {
            Name = name;
            Betters = new List<Preference>();
        }

        public IEnumerable<string> GetAllBetters()
        {
            var betterClassNames = Betters.Select(x => x.Name).ToList();
            foreach (var preference in Betters)
            {
                betterClassNames.AddRange(preference.GetAllBetters());
            }
            return betterClassNames;
        }
    }
}