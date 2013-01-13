using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public class PreferenceFileReader
    {
        private readonly string fileName;
        private Func<string, string, bool> inRelationFunc;
        public Dictionary<string, HashSet<string>> ClassRelations { get; set; }

        private readonly IDictionary<string, Preference> preferences = new Dictionary<string, Preference>(); 

        public PreferenceFileReader(string filename, RelationKind relationKind)
        {
            fileName = filename;
            ClassRelations = new Dictionary<string, HashSet<string>>();
            SetInRelationFunc(relationKind);
        }

        private void SetInRelationFunc(RelationKind relationKind)
        {
            switch (relationKind)
            {
                case RelationKind.Strict:
                    inRelationFunc = AreInStrictRelation;
                    break;
                case RelationKind.NonStrict:
                    inRelationFunc = AreInNonStrictRelation;
                    break;
                case RelationKind.Equal:
                    inRelationFunc = AreInEqualRelation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("relationKind");
            }
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
            var left = parts[0].Trim();
            var right = parts[1].Trim();
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

        public bool AreInRelation(string left, string right)
        {
            return inRelationFunc(left, right);
        }


        private bool AreInStrictRelation(string left, string right)
        {
            return ClassRelations[left].Contains(right);
        }

        private bool AreInNonStrictRelation(string left, string right)
        {
            return left == right || ClassRelations[left].Contains(right);
        }

        private bool AreInEqualRelation(string left, string right)
        {
            return left == right;
        }
    }
}
