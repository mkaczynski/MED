using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using UniversalPreferences.Algorithm;
using UniversalPreferences.App.Annotations;
using UniversalPreferences.App.Controls;
using UniversalPreferences.Common;
using UniversalPreferences.DAL;

namespace UniversalPreferences.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int viewIndex;
        private bool operationInProgress;
        private readonly IList<ViewContainer> views;

        private object content;
        public object Content
        {
            get { return content; }
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged("Content");
                }
            }
        }

        private string dataFilePath;
        private string dataFile;
        public string DataFile
        {
            get { return dataFile; }
            set
            {
                if (dataFile != value)
                {
                    dataFile = value;
                    OnPropertyChanged("DataFile");
                }
            }
        }

        private string relationFilePath;
        private string relationsFile;
        public string RelationsFile
        {
            get { return relationsFile; }
            set 
            {
                if (relationsFile != value)
                {
                    relationsFile = value;
                    OnPropertyChanged("RelationsFile");
                }
            }
        }

        public string DiagnosticsText { get; private set; }
        public string Results { get; private set; }
        public IList<AlgorithmDescription> Algorithms { get; private set; }
        public AlgorithmDescription Selected { get; set; }
        public int Index { get; set; }
        public string Separator { get; set; }
        public bool GoToResults { get; set; }
        public Visibility ProgressBarVisibility { get; private set; }
        public IList<RelationKind> Relations { get; private set; }
        public RelationKind SelectedRelation { get; set; }

        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }
        public ICommand SelectDataFileCommand { get; private set; }
        public ICommand SelectRelationsFileCommand { get; private set; }

        public MainViewModel()
        {
            views = new List<ViewContainer>();

            InitView();
            InitCommands();
            InitData();
            InitializeView();
        }

        private void InitView()
        {
            views.Add(new ViewContainer(
                new FilesSelection(),
                () => { },
                () => !string.IsNullOrWhiteSpace(RelationsFile) && !string.IsNullOrWhiteSpace(DataFile)));

            views.Add(new ViewContainer(new OptionsSelection(), () => { }, () => true));

            views.Add(new ViewContainer(new ProgressView(), OnStartCalculations, () => !operationInProgress));
            
            views.Add(new ViewContainer(new Results(), () => { }, () => false));
        }

        private void InitCommands()
        {
            NextCommand = new RelayCommand(OnNextCommand, x => viewIndex < views.Count - 1 && views[viewIndex].CanExecute());
            PrevCommand = new RelayCommand(OnPrevCommand, x => viewIndex > 0 && views[viewIndex].CanExecute());
            SelectDataFileCommand = new RelayCommand(OnSelectDataFileCommand);
            SelectRelationsFileCommand = new RelayCommand(OnSelectRelationsFileCommand);            
        }

        private void InitData()
        {
            DiagnosticsText = string.Empty;
            Results = string.Empty;

            dataFilePath = @"..\..\cardata.txt";
            DataFile = "cardata.txt";

            relationFilePath = @"..\..\relations.txt";
            RelationsFile = "relations.txt";

            Algorithms = 
                new List<AlgorithmDescription>
                    {
                        //new AlgorithmDescription(
                        //    "z generatorem", () => new Generators(new CandidatesGenerator())),
                        //new AlgorithmDescription(
                        //    "bez generatora", () => new ModifiedApriori(new CandidatesGenerator()))
                    };

            GoToResults = true;
            Selected = Algorithms[0];

            Relations = new List<RelationKind> 
                { RelationKind.Strict, RelationKind.NonStrict, RelationKind.Equal };
            SelectedRelation = Relations[0];
        }

        private void OnSelectRelationsFileCommand(object obj)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                relationFilePath = fileDialog.FileName;
                RelationsFile = Path.GetFileName(relationFilePath);
            }
        }

        private void OnSelectDataFileCommand(object obj)
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                dataFilePath = fileDialog.FileName;
                DataFile = Path.GetFileName(dataFilePath);
            }
        }

        private void OnPrevCommand(object obj)
        {
            viewIndex -= 1;
            InitializeView();
        }

        private void OnNextCommand(object obj)
        {
            viewIndex += 1;
            InitializeView();
        }

        private void InitializeView()
        {
            var tmp = views[viewIndex];
            tmp.Action();
            Content = tmp.View;
        }

        private void OnStartCalculations()
        {
            operationInProgress = true;
            DiagnosticsText = string.Empty;

            var manager = new ExecutionManager(Selected.Algorithm(),
                //new SimpleData());
                new CsvDataFileManager(dataFilePath, Separator, Index, relationFilePath, SelectedRelation));

            var bg = new BackgroundWorker();
            bg.DoWork += 
                (sender, args) =>
                    {
                        ProgressBarVisibility = Visibility.Visible;
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal, new Action(() => OnPropertyChanged("ProgressBarVisibility")));
                        
                        manager.DiagnosticsEvent += OnDiagnosticsEvent;
                        manager.Execute();
                        manager.DiagnosticsEvent -= OnDiagnosticsEvent;

                        Results = manager.GetResults();

                        ProgressBarVisibility = Visibility.Collapsed;
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal, new Action(() => OnPropertyChanged("ProgressBarVisibility")));
                        Application.Current.Dispatcher.Invoke(
                            DispatcherPriority.Normal, new Action(() => OnPropertyChanged("Results")));
                    };
            bg.RunWorkerCompleted += 
                (sender, args) =>
                    {
                        operationInProgress = false;
                        if (GoToResults)
                            NextCommand.Execute(new object());
                    };
        
            bg.RunWorkerAsync();
        }

        private void OnDiagnosticsEvent(object sender, DiagnosticsInfo e)
        {
            DiagnosticsText = DiagnosticsText + e.Info;

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(() => OnPropertyChanged("DiagnosticsText")));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}