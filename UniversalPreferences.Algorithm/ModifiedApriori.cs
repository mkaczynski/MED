namespace UniversalPreferences.Algorithm
{
    public class ModifiedApriori : BaseAlgorithm, IAlgorithm
    {
        public ModifiedApriori(int hashTreePageSize, int hashTreeKey, ICandidatesGenerator candidatesGenerator) : 
            base(hashTreePageSize, hashTreeKey, candidatesGenerator)
        {
        }
    }
}