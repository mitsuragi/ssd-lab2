using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Lab2.model;

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

        private int target;
        public int Target
        {
            get { return target; }
            set
            {
                target = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        public string? Result
        {
            get => Model.FindExpression(Target, numbers);
            set
            {
                OnPropertyChanged();
            }
        }

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
            ShowAboutCommand = new RelayCommand(ShowAbout, () => true);
        }

        public ICommand AddNumberCommand { get; }
        public ICommand RemoveNumberCommand { get; }
        public ICommand FileSaveInitialCommand { get; }
        public ICommand FileSaveResultCommand { get; }
        public ICommand FileLoadDataCommand { get; }
        public ICommand ShowAboutCommand { get; }

        private void ShowAbout()
        {
            string messageBoxText = "Задание выполнил студент группы №424 Губкин Максим.\n" +
                "Вариант №4\n\n" +
                "Текст задания: Используя знаки математических операций +, -, *, / из заданных чисел " +
                "постройте выражение, значение которого равно заданному.";

            string caption = "Справка";

            MessageBoxImage icon = MessageBoxImage.Information;

            MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, icon);
        }

        private void AddNumber()
        {
            numbers.Add(NumToAdd);
            OnPropertyChanged(nameof(Result));
        }

        private void RemoveNumber()
        {
            if (SelectedNumberIndex != -1)
            {
                numbers.RemoveAt(SelectedNumberIndex);
                OnPropertyChanged(nameof(Result));
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
                    else
                    {
                        numbers.Clear();
                        MessageBox.Show("Файл содержит некорректные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
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
