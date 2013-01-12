using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace UniversalPreferences.DAL
{
    public class PreferenceFileReader
    {
        private readonly string fileName;
        public Dictionary<string, HashSet<string>> ClassRelations { get; set; }

        private readonly IDictionary<string, Preference> preferences = new Dictionary<string, Preference>(); 

        public PreferenceFileReader(string filename)
        {
            fileName = filename;
            ClassRelations = new Dictionary<string, HashSet<string>>();
        }

        public void ProcessRelationsFile()
        {
            foreach (var line in File.ReadLines(fileName))
            {
                ProcessLine(line);
            }
            CreatePrefencesDict();
        }

        private void ProcessLine(string line)
        {
            string[] parts = line.Split('<');
            var left = parts[0];
            var right = parts[1];
            CreatePreferenceIfNoExists(left);
            CreatePreferenceIfNoExists(right);

            AddRelation(left, right);
        }

        private void AddRelation(string left, string right)
        {
            preferences[left].Betters.Add(preferences[right]);
        }

        private void CreatePreferenceIfNoExists(string name)
        {
            if(preferences.ContainsKey(name))
                return;
            preferences[name] = new Preference(name);
        }

        private void CreatePrefencesDict()
        {
            foreach (var preference in preferences.Values)
            {
                var betters = preference.GetAllBetters();
                ClassRelations[preference.Name] = new HashSet<string>(betters.Distinct().ToList());
            }
        }
    }
}
