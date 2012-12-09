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
        private IResultConverter resultConverter;

        private IList<Row> data;
        private IList<bool[]> preferences;

        public ExecutionManager(IAlgorithm algorithm)
        {
            this.algorithm = algorithm;
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
            var sd = new SimpleData();
            data = sd.GetData();

            var mappings = sd.GetMappings();
            resultConverter = new ResultConverter(mappings); // mozna by zrobic jakis IoC
        }
    }
}