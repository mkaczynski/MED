using System.Collections.Generic;

namespace UniversalPreferences.HashTree
{
    public interface ICandidateHashTree
    {
        void FillTree(IEnumerable<ushort[]> elements);

        IEnumerable<ushort[]> GetSupportedSets(ushort[] transaction);
        
    }
}