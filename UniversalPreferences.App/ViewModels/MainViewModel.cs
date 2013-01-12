using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using UniversalPreferences.App.Annotations;
using UniversalPreferences.App.Controls;

namespace UniversalPreferences.App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int viewIndex;
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

        public ICommand NextCommand { get; private set; }
        public ICommand PrevCommand { get; private set; }
        public ICommand SelectDataFileCommand { get; private set; }
        public ICommand SelectRelationsFileCommand { get; private set; }

        public MainViewModel()
        {
            views = new List<ViewContainer>
                        {
                            new ViewContainer(
                                new FilesSelection(), () => { }, () => !string.IsNullOrWhiteSpace(RelationsFile) && !string.IsNullOrWhiteSpace(DataFile)), 
                            new ViewContainer(new OptionsSelection(), () => {}, () => true), 
                            new ViewContainer(new Results(), () => { }, () => true)
                        };

            InitializeView();
        
            NextCommand = new RelayCommand(OnNextCommand, x => viewIndex < 2 && views[viewIndex].CanExecute());
            PrevCommand = new RelayCommand(OnPrevCommand, x => viewIndex > 0);
            SelectDataFileCommand = new RelayCommand(OnSelectDataFileCommand);
            SelectRelationsFileCommand = new RelayCommand(OnSelectRelationsFileCommand);
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}