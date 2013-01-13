using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public class CsvDataFileManager : IDataManager
    {
        private readonly CsvFileReader csvReader;
        private readonly PreferenceFileReader preferenceReader;

        public CsvDataFileManager(string fileNameObjects, string separator, int classNameColumnIndex, string fileNameRelations)
        {
            csvReader = new CsvFileReader(fileNameObjects, separator, classNameColumnIndex);
            preferenceReader = new PreferenceFileReader(fileNameRelations);
        }

        public void Initialize()
        {
            csvReader.ProcessFile();
            preferenceReader.ProcessRelationsFile();
        }

        public IList<Row> GetData()
        {
            List<Row> rowList = new List<Row>();
            for (int i = 0; i < csvReader.Rows.Count; ++i)
            {
                int numberOfAttributesFirst = csvReader.Rows[i].AttributeIds.Count;
                for (int j = 0; j < csvReader.Rows.Count; ++j)
                {
                    int numberOfAttributesSecond = csvReader.Rows[j].AttributeIds.Count;
                    ushort[] attributes = new ushort[numberOfAttributesFirst + numberOfAttributesSecond];
                    for (int k = 0; k < numberOfAttributesFirst; ++k)
                    {
                        attributes[k] = csvReader.Rows[i].AttributeIds[k];
                    }
                    for (int l = numberOfAttributesFirst; l < numberOfAttributesFirst + numberOfAttributesSecond; ++l)
                    {
                        attributes[l] = (ushort)(csvReader.Rows[j].AttributeIds[l - numberOfAttributesFirst] + csvReader.MaxAttributeId);
                    }

                    bool contains = preferenceReader.ClassRelations[csvReader.Rows[i].ClassName].Contains(csvReader.Rows[j].ClassName);
                    Relation value;
                    if (contains)
                        value = Relation.Complied;
                    else
                        value = Relation.NotComplied;
                    rowList.Add(new Row(csvReader.Rows[i].Id, csvReader.Rows[j].Id, attributes, value));
                }
            }
            return rowList;
        }

        public Dictionary<ushort, string> GetMappings()
        {
            return csvReader.Mapping;
        }

        public int MinLeftSideIndex()
        {
            return 7; //todo!  to powinna byc polowa mapowan + 1, ale tych  mapowan jest nieparzysta liczba!
        }
    }
}