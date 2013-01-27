using System;
using UniversalPreferences.Common;

namespace UniversalPreferences
{
    public class Arguments
    {
        public string DataFilePath { get; set; }
        public string RelationsFilePath { get; set; }

        public string Delimiter { get; set; }
        public int ClassIndex { get; set; }

        public int HashTreePageSize { get;  set; }
        public int HashTreeFirstNumber { get; set; }

        public Type Algorithm { get; set; }
        public RelationKind RelationKind { get; set; }

        public bool WriteIterationResults { get; set; }
        public string Method { get; set; } // P, T, G
    }
}