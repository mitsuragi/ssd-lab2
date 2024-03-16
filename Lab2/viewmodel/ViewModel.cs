using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab2.model;
using Microsoft.Win32;

namespace Lab2.viewmodel
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private int _target;
        public int Target
        {
            get { return _target; }
            set
            {
                _target = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public string? Result { get => Model.FindExpression(Target, numbers); }

        private ObservableCollection<int> numbers;

        public ObservableCollection<int> Numbers
        {
            get => numbers;
            set
            {
                numbers = value;
                OnPropertyChanged();
            }
        }

        private int numToAdd;
        public int NumToAdd
        {
            get => numToAdd;
            set
            {
                numToAdd = value;
                OnPropertyChanged();
            }
        }

        public int SelectedNumberIndex
        {
            get;
            set;
        }

        public ViewModel()
        {
            numbers = new ObservableCollection<int>();

            AddNumberCommand = new RelayCommand(AddNumber, () => true);
            RemoveNumberCommand = new RelayCommand(RemoveNumber, () => numbers.Count > 0);
            FileLoadDataCommand = new RelayCommand(FileLoadData, () => true);
            FileSaveInitialCommand = new RelayCommand(SaveInitialData, () => numbers.Count > 0);
            FileSaveResultCommand = new RelayCommand(SaveResultData, () => !string.IsNullOrWhiteSpace(Result));
        }

        public ICommand AddNumberCommand { get; }
        public ICommand RemoveNumberCommand { get; }
        public ICommand FileSaveInitialCommand { get; }
        public ICommand FileSaveResultCommand { get; }
        public ICommand FileLoadDataCommand { get; }

        
        private void AddNumber()
        {
            numbers.Add(NumToAdd);
        }

        private void RemoveNumber()
        {
            if (SelectedNumberIndex != -1)
            {
                numbers.RemoveAt(SelectedNumberIndex);
            } 
            else
            {
                return;
            }
        }

        private void SaveInitialData()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Исходные данные";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Текстовые документы (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                WriteInitialDataToFile(filename);
            }
        }

        private void SaveResultData()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Результат";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Текстовые документы (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                WriteResultDataToFile(filename);
            }
        }

        private void FileLoadData()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Текстовые документы (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                GetData(filename);
            }
        }
        private void GetData(string filename)
        {
            numbers.Clear();

            StreamReader sr = new StreamReader(filename);

            string? line;
            
            while ((line = sr.ReadLine()) != null)
            {
                string[] numberStrings = line.Split(' ');
                foreach (string numberString in numberStrings)
                {
                    if (int.TryParse(numberString, out int number))
                    {
                        numbers.Add(number);
                    }
                }
            }

            sr.Close();
        }

        private void WriteInitialDataToFile(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);

            string line = string.Join(" ", numbers);

            sw.Write(line);

            sw.Close();
        }

        private void WriteResultDataToFile(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);

            sw.Write(Result);

            sw.Close();
        }
    }
}
