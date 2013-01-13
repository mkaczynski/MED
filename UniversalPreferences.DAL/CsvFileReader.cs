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

        public ushort MaxAttributeId { get; private set; }

        public CsvFileReader(string fileName, string separator, int classNameIndex)
        {
            this.fileName = fileName;
            this.separators = new[] {separator};
            this.classNameIndex = classNameIndex;
            Rows = new List<InternalRow>();
            internalMapping = new Dictionary<string, ushort>();
            Mapping = new Dictionary<ushort, string>();
            ClassNames = new List<string>();
        }

        public void ProcessFile()
        {
            foreach (var line in File.ReadLines(fileName))
            {
                ProcessLine(line);
            }
            SetMaxAttributeId();
            CreateMapping();
        }

        private void ProcessLine(string line)
        {
            var splited = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string className = splited[classNameIndex].Trim();
            AddClassIfNoExists(className);

            var row = new InternalRow(currentRowId++, className);

            for (int i = 0; i < splited.Length; i++)
            {
                if (i == classNameIndex)
                    continue;

                if(string.IsNullOrWhiteSpace(splited[i]))
                    continue;

                var key = CreateAttributeKey(splited, i);
                AddAtributeMappinigIfNoExists(key);

                var attributeId = internalMapping[key];
                row.AddAttributeId(attributeId);
            }
            row.SortAttibutes();
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
            return string.Format("{0}#{1}", i, splited[i].Trim());
        }

        private void SetMaxAttributeId()
        {
            MaxAttributeId = currentId;
        }

        private void CreateMapping()
        {
            foreach (var keyToIndex in internalMapping)
            {
                Mapping[keyToIndex.Value] = keyToIndex.Key;
                Mapping[(ushort)(keyToIndex.Value + MaxAttributeId)] = keyToIndex.Key;
            }
        }
    }
}