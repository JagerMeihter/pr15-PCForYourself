using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using pr15_PCForYourself.Models;

namespace pr15_PCForYourself.ViewModels
{
    public class SavedBuildsViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainVM;
        private SavedBuild _selectedBuild;

        public ObservableCollection<SavedBuild> SavedBuilds { get { return _mainVM.SavedBuilds; } }

        public SavedBuild SelectedBuild
        {
            get { return _selectedBuild; }
            set { _selectedBuild = value; OnPropertyChanged(); }
        }

        public RelayCommand LoadBuildCommand { get; }
        public RelayCommand DeleteBuildCommand { get; }

        public SavedBuildsViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            LoadBuildCommand = new RelayCommand(LoadBuild, _ => SelectedBuild != null);
            DeleteBuildCommand = new RelayCommand(DeleteBuild, _ => SelectedBuild != null);
        }

        private void LoadBuild(object param)
        {
            if (SelectedBuild == null) return;

            // Очищаем текущую сборку
            _mainVM.CurrentBuild.SelectedComponents.Clear();
            foreach (var kv in SelectedBuild.Components)
            {
                if (kv.Value != null)
                    _mainVM.CurrentBuild.SelectedComponents[kv.Key] = kv.Value;
            }

            MessageBox.Show("Сборка загружена в текущую конфигурацию!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteBuild(object param)
        {
            if (SelectedBuild == null) return;
            _mainVM.SavedBuilds.Remove(SelectedBuild);
            SelectedBuild = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}