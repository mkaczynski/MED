using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UniversalPreferences.DAL
{
    public class PreferenceFileReader
    {
        private readonly string fileName;
        public Dictionary<string, HashSet<string>> ClassRelations { get; set; }


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

            FindTransitivePreferences();
        }

        private void ProcessLine(string line)
        {
            string[] parts = line.Split('<');
            if(!ClassRelations.Keys.Contains(parts[0]))
                ClassRelations.Add(parts[0], new HashSet<string>());
            ClassRelations[parts[0]].Add(parts[1]);
            ClassRelations.Add(parts[1], new HashSet<string>());
        }

        private void FindTransitivePreferences()
        {
            var tempDict = new Dictionary<string, HashSet<string>>();
            foreach (var item in ClassRelations.Reverse())
            {
                tempDict.Add(item.Key, new HashSet<string>());
                foreach( string s in item.Value)
                {
                    foreach (string s2 in ClassRelations[s])
                    {
                        if (!item.Value.Contains(s2))
                            tempDict[item.Key].Add(s2);  
                    }
                    
                }
                ClassRelations[item.Key].UnionWith(tempDict[item.Key]);
            }
        }
        
    }
}
