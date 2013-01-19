using System.Collections.Generic;

namespace UniversalPreferences.HashTree
{
    public interface ICandidateHashTree
    {
        void FillTree(IList<ushort[]> elements);

        IEnumerable<ushort[]> GetSupportedSets(ushort[] transaction);
        
    }
}