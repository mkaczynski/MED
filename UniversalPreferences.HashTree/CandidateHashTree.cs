using System.Collections.Generic;

namespace UniversalPreferences.HashTree
{
    class CandidateHashTree : ICandidateHashTree
    {
        private readonly CandidateNode root;

        internal CandidateHashTree(int transactionLength, int pageSize, int firstNumber)
        {
            root = new CandidateNode(pageSize, firstNumber, transactionLength, 0);
        }

        public void FillTree(IEnumerable<ushort[]> elements)
        {
            foreach (var simpleRow in elements)
            {
                root.Add(simpleRow);
            }
        }

        public IEnumerable<ushort[]> GetSupportedSets(ushort[] transaction)
        {
            var supportedRows = new List<ushort[]>();
            root.FillSupportedRows(supportedRows, transaction, 0, new HashSet<ushort>(transaction));
            return supportedRows;
        }
    }
}