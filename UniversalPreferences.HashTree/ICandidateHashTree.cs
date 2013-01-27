using System.Collections.Generic;
using UniversalPreferences.Common;

namespace UniversalPreferences.HashTree
{
    public interface ICandidateHashTree
    {
        void FillTree(IList<SimpleRow> elements);

        IEnumerable<SimpleRow> GetSupportedSets(SimpleRow transaction);
        
    }
}