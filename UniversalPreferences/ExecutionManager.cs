using System;
using System.Collections.Generic;
using System.Linq;
using UniversalPreferences.Algorithm;
using UniversalPreferences.Common;
using UniversalPreferences.DAL;

namespace UniversalPreferences
{
    public class ExecutionManager
    {
        private readonly IAlgorithm algorithm;
        private readonly IDataManager dataManager;
        private IResultConverter resultConverter;

        private IList<Row> data;
        private IList<bool[]> preferences;

        public ExecutionManager(IAlgorithm algorithm, IDataManager dataManager)
        {
            this.algorithm = algorithm;
            this.dataManager = dataManager;
        }

        public void Execute()
        {
            GetData();
            CalculatePreferences();
            ShowResults();
        }

        private void ShowResults()
        {
            foreach(var preference in preferences)
            {
                var res = resultConverter.Convert(preference);
                Console.WriteLine(res);
            }
        }

        private void CalculatePreferences()
        {
            preferences = new List<bool[]>(algorithm.FindPreferences(data));
        }
        
        private void GetData()
        {
            dataManager.Initialize();
            data = dataManager.GetData();

            var mappings = dataManager.GetMappings();
            resultConverter = new ResultConverter(mappings);
        }
    }
}