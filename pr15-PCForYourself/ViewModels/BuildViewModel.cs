using pr15_PCForYourself.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace pr15_PCForYourself.ViewModels
{
    public class BuildViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainVM;
        private PCComponent _selectedComponent;

        public ObservableCollection<PCComponent> FilteredComponents { get; set; }
        public ObservableCollection<string> Manufacturers { get; set; }

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; Filter(); OnPropertyChanged(); }
        }

        private string _selectedManufacturer;
        public string SelectedManufacturer
        {
            get { return _selectedManufacturer; }
            set { _selectedManufacturer = value; Filter(); OnPropertyChanged(); }
        }

        public PCComponent SelectedComponent
        {
            get { return _selectedComponent; }
            set { _selectedComponent = value; OnPropertyChanged(); }
        }

        public ComputerBuild CurrentBuild { get { return _mainVM.CurrentBuild; } }

        public RelayCommand AddComponentCommand { get; }
        public RelayCommand RemoveComponentCommand { get; }
        public RelayCommand SaveBuildCommand { get; }

        public BuildViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;

            Manufacturers = new ObservableCollection<string>(
                _mainVM.AllComponents.Select(c => c.Manufacturer).Distinct().OrderBy(m => m)
            );
            Manufacturers.Insert(0, "Все производители");

            FilteredComponents = new ObservableCollection<PCComponent>(_mainVM.AllComponents);

            AddComponentCommand = new RelayCommand(AddComponent, CanAddComponent);
            RemoveComponentCommand = new RelayCommand(RemoveComponent);
            SaveBuildCommand = new RelayCommand(SaveBuild);
        }

        private void Filter()
        {
            var query = _mainVM.AllComponents.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(c => c.Name.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);

            if (!string.IsNullOrEmpty(SelectedManufacturer) && SelectedManufacturer != "Все производители")
                query = query.Where(c => c.Manufacturer == SelectedManufacturer);

            FilteredComponents.Clear();
            foreach (var item in query)
                FilteredComponents.Add(item);
        }

        private bool CanAddComponent(object param)
        {
            if (SelectedComponent == null) return false;
            return !CurrentBuild.SelectedComponents.ContainsKey(SelectedComponent.Category) ||
                   CurrentBuild.SelectedComponents[SelectedComponent.Category] == null;
        }

        private void AddComponent(object param)
        {
            if (SelectedComponent == null) return;

            if (!CheckCompatibility(SelectedComponent, SelectedComponent.Category))
            {
                MessageBox.Show("Данный компонент несовместим с уже выбранными!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentBuild.SelectedComponents[SelectedComponent.Category] = SelectedComponent;
            OnPropertyChanged(nameof(CurrentBuild));
            CommandManager.InvalidateRequerySuggested();
        }

        private void RemoveComponent(object param)
        {
            if (param is PCComponent component)
            {
                var category = component.Category;
                if (CurrentBuild.SelectedComponents.ContainsKey(category))
                {
                    CurrentBuild.SelectedComponents[category] = null;
                    OnPropertyChanged(nameof(CurrentBuild));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        private void SaveBuild(object param)
        {
            if (CurrentBuild.SelectedComponents.Count == 0)
            {
                MessageBox.Show("Сборка пуста!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new Views.SaveBuildDialog();
            if (dialog.ShowDialog() == true)
            {
                var saved = new SavedBuild
                {
                    Name = dialog.BuildName,
                    Author = dialog.Author,
                    Components = new Dictionary<ComponentCategory, PCComponent>(CurrentBuild.SelectedComponents),
                    TotalPrice = CurrentBuild.TotalPrice
                };
                _mainVM.SavedBuilds.Add(saved);
                MessageBox.Show("Сборка сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckCompatibility(PCComponent newComp, ComponentCategory category)
        {
            var build = CurrentBuild;

            switch (category)
            {
                case ComponentCategory.CPU:
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.Motherboard))
                    {
                        var mb = build.SelectedComponents[ComponentCategory.Motherboard];
                        if (mb != null && mb.Socket != newComp.Socket)
                            return false;
                    }
                    break;

                case ComponentCategory.Motherboard:
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.CPU))
                    {
                        var cpu = build.SelectedComponents[ComponentCategory.CPU];
                        if (cpu != null && cpu.Socket != newComp.Socket)
                            return false;
                    }

                    if (build.SelectedComponents.ContainsKey(ComponentCategory.RAM))
                    {
                        var ram = build.SelectedComponents[ComponentCategory.RAM];
                        if (ram != null && ram.MemoryType != newComp.MemoryType)
                            return false;
                    }

                    if (build.SelectedComponents.ContainsKey(ComponentCategory.Case))
                    {
                        var pcCase = build.SelectedComponents[ComponentCategory.Case];
                        if (pcCase != null && pcCase.SupportedFormFactors != null &&
                            !pcCase.SupportedFormFactors.Contains(newComp.FormFactor))
                            return false;
                    }
                    break;

                case ComponentCategory.RAM:
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.Motherboard))
                    {
                        var mbRam = build.SelectedComponents[ComponentCategory.Motherboard];
                        if (mbRam != null && mbRam.MemoryType != newComp.MemoryType)
                            return false;
                    }
                    break;

                case ComponentCategory.Cooler:
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.CPU))
                    {
                        var cpuCooler = build.SelectedComponents[ComponentCategory.CPU];
                        if (cpuCooler != null && newComp.SupportedSockets != null &&
                            !newComp.SupportedSockets.Contains(cpuCooler.Socket))
                            return false;
                    }
                    break;

                case ComponentCategory.Case:
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.Motherboard))
                    {
                        var mbCase = build.SelectedComponents[ComponentCategory.Motherboard];
                        if (mbCase != null && newComp.SupportedFormFactors != null &&
                            !newComp.SupportedFormFactors.Contains(mbCase.FormFactor))
                            return false;
                    }
                    break;

                case ComponentCategory.GPU:
                    break;

                case ComponentCategory.PSU:
                    int totalPower = 0;
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.CPU))
                    {
                        var cpuPower = build.SelectedComponents[ComponentCategory.CPU];
                        if (cpuPower != null)
                            totalPower += cpuPower.PowerConsumption ?? 0;
                    }
                    if (build.SelectedComponents.ContainsKey(ComponentCategory.GPU))
                    {
                        var gpuPower = build.SelectedComponents[ComponentCategory.GPU];
                        if (gpuPower != null)
                            totalPower += gpuPower.PowerConsumption ?? 0;
                    }
                    totalPower += 50;

                    if (newComp.PowerCapacity < totalPower)
                        return false;
                    break;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}