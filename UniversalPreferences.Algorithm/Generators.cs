using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class Generators : BaseAlgorithm, IAlgorithm
    {
        //private readonly Dictionary<string, SimpleRow> eachResults;

        public Generators(int hashTreePageSize, int hashTreeKey, bool writeIterationResultsToFile, string method,
            ICandidatesGenerator candidatesGenerator, IResultConverter resultConverter) 
            : base(hashTreePageSize, hashTreeKey, writeIterationResultsToFile, method,
                candidatesGenerator, resultConverter)
        {
            //eachResults = new Dictionary<string, SimpleRow>();
        }

        protected override void Initialize(IEnumerable<Row> transactions)
        {
            //eachResults.Add(string.Empty, new SimpleRow(new ushort[0]) { RelationComplied = transactions.Count() });
        }

        protected override void OnAddNode(SimpleRow row)
        {
            //var description = GetDescription(row.Transaction);
            //eachResults.Add(description, row); 
        }

        //protected override bool CheckIfAnySubsetIsGenerator(SimpleRow row)
        //{
            //var subsets = GetSubsets(row.Transaction);
            //foreach (var subset in subsets)
            //{
            //    var support = 0;
            //    var description = GetDescription(subset);
            //    if (eachResults.ContainsKey(description))
            //    {
            //        var value = eachResults[description];
            //        support = value.RelationComplied + value.RelationNotComplied;
            //    }

            //    if (support == row.RelationComplied + row.RelationNotComplied)
            //        return true;
            //}

            //return false;
        //}

        //private IEnumerable<ushort[]> GetSubsets(IEnumerable<ushort> set)
        //{
        //    var subsets = new List<ushort[]>();
        //    GetSubsets(set, subsets);
        //    return subsets;
        //}

        //private static void GetSubsets(IEnumerable<ushort> set, IList<ushort[]> subsets)
        //{
        //    var tmp = set.Select((t, i) => set.Take(i).Concat(set.Skip(i + 1)).ToArray()).ToList();
        //    tmp.ForEach(subsets.Add);   
        //}
    }
}