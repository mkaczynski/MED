using System.Text;
using UniversalPreferences.DAL;

namespace UniversalPreferences.Algorithm
{
    public class ResultConverter : IResultConverter
    {
        private readonly IDataManager dataManager;

        public ResultConverter(IDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public string Convert(ushort[] preferences)
        {
            var sb = new StringBuilder();
            var firstProcessed = false;

            sb.Append("(");
            for (int i = 0; i < preferences.Length; ++i)
            {
                sb.Append(dataManager.GetMappings()[preferences[i]]);

                if (!firstProcessed && preferences[i + 1] >= dataManager.MinLeftSideIndex() &&
                    preferences[i] < dataManager.MinLeftSideIndex())
                {
                    firstProcessed = true;
                    sb.Append(") => (");
                }
                else if(i < preferences.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}