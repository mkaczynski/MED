using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public class CsvDataFileManager : IDataManager
    {
        private readonly CsvFileReader reader;

        public CsvDataFileManager(string fileName, string separator, int classNameColumnIndex)
        {
            reader = new CsvFileReader(fileName, separator, classNameColumnIndex);
        }

        public void Initialize()
        {
            reader.ProcessFile();
        }

        public IList<Row> GetData()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<int, string> GetMappings()
        {
            return reader.Mapping;
        }

        public int MinLeftSideIndex()
        {
            throw new System.NotImplementedException();
        }
    }
}