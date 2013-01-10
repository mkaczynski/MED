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
            reader.ProcessRelationsFile();
        }

        public IList<Row> GetData()
        {
            List<Row> rowList = new List<Row>();
            for (int i = 0; i < reader.Rows.Count; ++i)
            {
                int numberOfAttributesFirst = reader.Rows[i].AttributeIds.Count;
                for (int j = 0; j < reader.Rows.Count; ++j)
                {
                    int numberOfAttributesSecond = reader.Rows[j].AttributeIds.Count;
                    ushort[] attributes = new ushort[numberOfAttributesFirst + numberOfAttributesSecond];
                    for (int k = 0; k < numberOfAttributesFirst; ++k)
                    {
                        attributes[k] = reader.Rows[i].AttributeIds[k];
                    }
                    for (int l = numberOfAttributesFirst; l < numberOfAttributesFirst + numberOfAttributesSecond; ++l)
                    {
                        attributes[l] = reader.Rows[j].AttributeIds[l - numberOfAttributesFirst];
                    }

                    bool contains = reader.ClassRelations[reader.Rows[i].ClassName].Contains(reader.Rows[j].ClassName);
                    Relation value;
                    if (contains)
                        value = Relation.Complied;
                    else
                        value = Relation.NotComplied;
                    rowList.Add(new Row(reader.Rows[i].Id, reader.Rows[j].Id, attributes, value));
                }
            }
            return rowList;
        }

        public Dictionary<ushort, string> GetMappings()
        {
            return reader.Mapping;
        }

        public int MinLeftSideIndex()
        {
            //throw new System.NotImplementedException();
            return 7;
        }
    }
}