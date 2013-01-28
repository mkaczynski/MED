using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniversalPreferences.Common;

namespace UniversalPreferences.DAL
{
    public class CsvDataFileManager : IDataManager
    {
        private readonly string preferenceMatrix;
        private readonly CsvFileReader csvReader;
        private readonly PreferenceFileReader preferenceReader;

        public CsvDataFileManager(string fileNameObjects, string separator, int classNameColumnIndex, string fileNameRelations, RelationKind kind, string preferenceMatrix)
        {
            this.preferenceMatrix = preferenceMatrix;
            csvReader = new CsvFileReader(fileNameObjects, separator, classNameColumnIndex);
            preferenceReader = new PreferenceFileReader(fileNameRelations, kind);
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

                    bool contains = preferenceReader.AreInRelation(csvReader.Rows[i].ClassName, csvReader.Rows[j].ClassName);
                    Relation value;
                    if (contains)
                        value = Relation.Complied;
                    else
                        value = Relation.NotComplied;
                    rowList.Add(new Row(csvReader.Rows[i].Id, csvReader.Rows[j].Id, attributes, value));
                }
            }
            if(preferenceMatrix != null)
            {
                WritePreferenceMatriToFile(rowList);
            }
            return rowList;
        }

        private void WritePreferenceMatriToFile(List<Row> rowList)
        {
            using (var writer = new StreamWriter(preferenceMatrix, false))
            {
                foreach (var row in rowList)
                {
                    var line = BuildPreferenceMatrixLine(row);
                    writer.WriteLine(line);
                }
            }
        }

        private string BuildPreferenceMatrixLine(Row row)
        {
            StringBuilder sb = new StringBuilder();
            var left = string.Join(", ", row.Attributes.Where(x => x + 1< MinLeftSideIndex()).Select(x => GetMappings()[x]));
            sb.Append(left);
            sb.AppendFormat(" | {0} | ", row.Value == Relation.Complied ? "x" : " ");
            var right = string.Join(", ", row.Attributes.Where(x => x + 1>= MinLeftSideIndex()).Select(x => GetMappings()[x]));
            sb.Append(right);
            return sb.ToString();
        }

        public Dictionary<ushort, string> GetMappings()
        {
            return csvReader.Mapping;
        }

        public int MinLeftSideIndex()
        {
            return csvReader.Mapping.Count/2 + 1;
        }
    }
}