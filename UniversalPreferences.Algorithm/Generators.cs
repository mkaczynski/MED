using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Common;

namespace UniversalPreferences.Algorithm
{
    public class Generators : BaseAlgorithm, IAlgorithm
    {
        private readonly Dictionary<string, SimpleRow> eachResults; 

        public Generators(ICandidatesGenerator candidatesGenerator) 
            : base(candidatesGenerator)
        {
            eachResults = new Dictionary<string, SimpleRow>();
        }

        protected override void Initialize(IEnumerable<Row> transactions)
        {
            eachResults.Add(string.Empty, new SimpleRow(null) { RelationComplied = transactions.Count() });
        }

        protected override void OnAddNode(string description, SimpleRow row)
        {
            if (!eachResults.ContainsKey(description))
            {
                eachResults.Add(description, row);
            }
        }

        protected override bool CheckIfAnySubsetIsGenerator(SimpleRow row)
        {
            var subsets = GetSubsets(row.Transaction);
            foreach (var subset in subsets)
            {
                var support = 0;
                var description = GetDescription(subset);
                if (eachResults.ContainsKey(description))
                {
                    var value = eachResults[description];
                    support = value.RelationComplied + value.RelationNotComplied;
                }

                if (support == row.RelationComplied + row.RelationNotComplied)
                    return true;
            }

            return false;
        }

        private IEnumerable<ushort[]> GetSubsets(ushort[] set)
        {
            var subsets = set.Select((t, i) => set.Take(i).Concat(set.Skip(i + 1)).ToArray()).ToList();
            return subsets;
        }
    }
}