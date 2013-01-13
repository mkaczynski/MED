using System;
using System.Collections.Generic;
using System.Text;
using UniversalPreferences.Algorithm;
using UniversalPreferences.Common;
using UniversalPreferences.DAL;

namespace UniversalPreferences.App.ViewModels
{
    public class ExecutionManager
    {
        public event EventHandler<DiagnosticsInfo> DiagnosticsEvent;

        private readonly IAlgorithm algorithm;
        private readonly IDataManager dataManager;
        private IResultConverter resultConverter;

        private IList<Row> data;
        private IList<IEnumerable<ushort>> preferences;

        public ExecutionManager(IAlgorithm algorithm, IDataManager dataManager)
        {
            this.algorithm = algorithm;
            this.dataManager = dataManager;

            algorithm.DiagnosticsEvent += OnInternalDiagnosticsEvent;
        }

        private void OnInternalDiagnosticsEvent(object sender, DiagnosticsInfo info)
        {
            OnDiagnosticsEvent(info);
        }

        public void Execute()
        {
            GetData();
            CalculatePreferences();
            //ShowResults();
        }

        public string GetResults()
        {
            int i = 0;
            var sb = new StringBuilder();
            foreach (var preference in preferences)
            {
                var res = resultConverter.Convert(preference);
                sb.AppendLine(string.Format("{0}. {1}", ++i, res));
                //Console.WriteLine(res);
            }
            return sb.ToString();
        }

        //private void ShowResults()
        //{
        //    foreach(var preference in preferences)
        //    {
        //        var res = resultConverter.Convert(preference);
        //        //Console.WriteLine(res);
        //    }
        //}

        private void CalculatePreferences()
        {
            preferences = new List<IEnumerable<ushort>>(algorithm.FindPreferences(data));
        }
        
        private void GetData()
        {
            dataManager.Initialize();
            data = dataManager.GetData();

            resultConverter = new ResultConverter(dataManager);
        }

        protected virtual void OnDiagnosticsEvent(DiagnosticsInfo e)
        {
            EventHandler<DiagnosticsInfo> handler = DiagnosticsEvent;
            if (handler != null) handler(this, e);
        }
    }
}