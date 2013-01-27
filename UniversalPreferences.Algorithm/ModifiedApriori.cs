namespace UniversalPreferences.Algorithm
{
    public class ModifiedApriori : BaseAlgorithm, IAlgorithm
    {
        public ModifiedApriori(int hashTreePageSize, int hashTreeKey, bool writeIterationResultsToFile, string method,
            ICandidatesGenerator candidatesGenerator, IResultConverter resultConverter) : 
            base(hashTreePageSize, hashTreeKey, writeIterationResultsToFile, method, candidatesGenerator, resultConverter)
        {
        }
    }
}