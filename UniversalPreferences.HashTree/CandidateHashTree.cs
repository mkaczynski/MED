using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    class CandidateHashTree : ICandidateHashTree
    {
        private readonly CandidateNode root;

        internal CandidateHashTree(int transactionLength, int pageSize, int firstNumber)
        {
            root = new CandidateNode(pageSize, firstNumber, transactionLength, 0);
        }

        public void FillTree(IList<SimpleRow> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
            }
        }

        public IEnumerable<SimpleRow> GetSupportedSets(SimpleRow transaction)
        {
            var supportedRows = new List<SimpleRow>();
            root.FillSupportedRows(supportedRows, transaction, 0, new HashSet<ushort>(transaction.Transaction));
            return supportedRows;
        }
    }
}