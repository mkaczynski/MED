using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UniversalPreferences.DAL
{
    class CsvFileReader
    {
        private readonly string fileName;
        private readonly string[] separators;
        private readonly int classNameIndex;

        private readonly IDictionary<string, ushort> internalMapping;
        private ushort currentId;
        private int currentRowId;

        public IList<InternalRow> Rows { get; private set; }

        public Dictionary<ushort,string> Mapping { get; set; }

        public IList<string> ClassNames { get; private set; }

        public Dictionary<string, HashSet<string>> ClassRelations { get; set; }

        public CsvFileReader(string fileName, string separator, int classNameIndex)
        {
            this.fileName = fileName;
            this.separators = new[] {separator};
            this.classNameIndex = classNameIndex;
            Rows = new List<InternalRow>();
            internalMapping = new Dictionary<string, ushort>();
            Mapping = new Dictionary<ushort, string>();
            ClassNames = new List<string>();
            ClassRelations = new Dictionary<string, HashSet<string>>();
        }

        public void ProcessFile()
        {
            foreach (var line in File.ReadLines(fileName))
            {
                ProcessLine(line);
            }
            CreateMapping();
        }

        public void ProcessRelationsFile()
        {
            string line;
            string[] parts = null;
            System.IO.StreamReader file =
               new System.IO.StreamReader(@"C:\Documents and Settings\Bartek\Moje dokumenty\MED\UniversalPreferences\relations.txt");

            while ((line = file.ReadLine()) != null)
            {
                parts = line.Split('<');
                ClassRelations.Add(parts[0], new HashSet<string>());
                ClassRelations[parts[0]].Add(parts[1]);
            }
            ClassRelations[parts[1]] = new HashSet<string>();

            int cnt = 0;
            foreach (var item in ClassRelations.Reverse())
            {
                if(item.Value.Count !=0)
                {
                    foreach (string s2 in ClassRelations[ClassRelations.Keys.ElementAt(ClassRelations.Keys.Count -1-(cnt++))])
                    {
                        if (!item.Value.Contains(s2))
                            ClassRelations[item.Key].Add(s2);
                    }
                }
            }
          
            file.Close();

            Console.WriteLine("Finished processing relations file");

        }

        private void ProcessLine(string line)
        {
            var splited = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string className = splited[classNameIndex];
            AddClassIfNoExists(className);

            var row = new InternalRow(currentRowId++, className);

            for (int i = 0; i < splited.Length; i++)
            {
                if (i == classNameIndex)
                    continue;
                var key = CreateAttributeKey(splited, i);
                AddAtributeMappinigIfNoExists(key);

                var attributeId = internalMapping[key];
                row.AddAttributeId(attributeId);
            }

            Rows.Add(row);
        }

        private void AddClassIfNoExists(string className)
        {
            if(!ClassNames.Contains(className))
            {
                ClassNames.Add(className);
            }
        }

        private void AddAtributeMappinigIfNoExists(string key)
        {
            if (!internalMapping.ContainsKey(key))
                internalMapping[key] = currentId++;
        }

        private static string CreateAttributeKey(string[] splited, int i)
        {
            return string.Format("{0}#{1}", i, splited[i]);
        }

        private void CreateMapping()
        {
            foreach (var keyToIndex in internalMapping)
            {
                Mapping[keyToIndex.Value] = keyToIndex.Key;
            }
        }
    }
}