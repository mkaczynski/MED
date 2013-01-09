using System;
using System.Collections.Generic;
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
        private IList<ushort[]> preferences;

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
            Console.ReadLine();
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
            preferences = new List<ushort[]>(algorithm.FindPreferences(data));
        }
        
        private void GetData()
        {
            dataManager.Initialize();   
            data = dataManager.GetData(); 

            resultConverter = new ResultConverter(dataManager);
        }
    }
}