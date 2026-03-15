using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using pr15_PCForYourself.Models;

namespace pr15_PCForYourself.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PCComponent> AllComponents { get; set; }
        public ObservableCollection<SavedBuild> SavedBuilds { get; set; }
        public ComputerBuild CurrentBuild { get; set; }

        public Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public RelayCommand NavigateToBuildCommand { get; }
        public RelayCommand NavigateToSavedCommand { get; }

        public MainViewModel()
        {
            LoadTestData(); // Заглушка, замените на загрузку из БД

            CurrentBuild = new ComputerBuild();
            SavedBuilds = new ObservableCollection<SavedBuild>();

            var buildVM = new BuildViewModel(this);
            var savedVM = new SavedBuildsViewModel(this);

            NavigateToBuildCommand = new RelayCommand(_ => CurrentPage = new Views.BuildPage { DataContext = buildVM });
            NavigateToSavedCommand = new RelayCommand(_ => CurrentPage = new Views.SavedBuildsPage { DataContext = savedVM });

            // Стартовая страница
            NavigateToBuildCommand.Execute(null);
        }

        public void LoadTestData()
        {
            AllComponents = new ObservableCollection<PCComponent>
            {
                new PCComponent { Id=1, Name="Intel Core i5-12400", Manufacturer="Intel", Price=200, Category=ComponentCategory.CPU, Socket="LGA1700", PowerConsumption=65 },
                new PCComponent { Id=2, Name="AMD Ryzen 5 5600X", Manufacturer="AMD", Price=220, Category=ComponentCategory.CPU, Socket="AM4", PowerConsumption=65 },
                new PCComponent { Id=3, Name="ASUS Prime B660-PLUS", Manufacturer="ASUS", Price=150, Category=ComponentCategory.Motherboard, Socket="LGA1700", FormFactor="ATX", MemoryType="DDR4" },
                new PCComponent { Id=4, Name="MSI B550 TOMAHAWK", Manufacturer="MSI", Price=160, Category=ComponentCategory.Motherboard, Socket="AM4", FormFactor="ATX", MemoryType="DDR4" },
                new PCComponent { Id=5, Name="Kingston DDR4 3200 16GB", Manufacturer="Kingston", Price=80, Category=ComponentCategory.RAM, MemoryType="DDR4" },
                new PCComponent { Id=6, Name="Corsair Vengeance DDR4 3600 16GB", Manufacturer="Corsair", Price=95, Category=ComponentCategory.RAM, MemoryType="DDR4" },
                new PCComponent { Id=7, Name="NVIDIA RTX 3060", Manufacturer="NVIDIA", Price=350, Category=ComponentCategory.GPU, PowerConsumption=170 },
                new PCComponent { Id=8, Name="AMD Radeon RX 6600", Manufacturer="AMD", Price=320, Category=ComponentCategory.GPU, PowerConsumption=132 },
                new PCComponent { Id=9, Name="be quiet! Dark Rock 4", Manufacturer="be quiet!", Price=70, Category=ComponentCategory.Cooler, SupportedSockets = new System.Collections.Generic.List<string>{"LGA1700","AM4"} },
                new PCComponent { Id=10, Name="Corsair RM750", Manufacturer="Corsair", Price=120, Category=ComponentCategory.PSU, PowerCapacity=750 },
                new PCComponent { Id=11, Name="Deepcool MATREXX 55", Manufacturer="Deepcool", Price=60, Category=ComponentCategory.Case, SupportedFormFactors = new System.Collections.Generic.List<string>{"ATX","Micro-ATX"} }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}