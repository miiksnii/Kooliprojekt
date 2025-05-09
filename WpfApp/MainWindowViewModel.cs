using System.Collections.ObjectModel;
using System.Windows.Input;
using KooliProjekt.WpfApp.Api;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<WorkLog> Lists { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public Predicate<WorkLog> ConfirmDelete { get; set; }

        private readonly IWorklogApiClient _apiClient;

        private WorkLog _selectedItem;
        public WorkLog SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindowViewModel() : this(new WorklogApiClient())
        {
        }

        public MainWindowViewModel(IWorklogApiClient apiClient)
        {
            _apiClient = apiClient;

            Lists = new ObservableCollection<WorkLog>();

            NewCommand = new RelayCommand<WorkLog>(list =>
            {
                SelectedItem = new WorkLog(); // Create a new empty WorkLog when 'New' is clicked
            });

            SaveCommand = new RelayCommand<WorkLog>(async list =>
            {
                if (SelectedItem != null)
                {
                    await _apiClient.Save(SelectedItem);
                    await Load();
                }
            }, list => SelectedItem != null); // CanExecute is based on whether SelectedItem is null

            DeleteCommand = new RelayCommand<WorkLog>(async list =>
            {
                if (ConfirmDelete != null)
                {
                    var result = ConfirmDelete(SelectedItem);
                    if (!result) return;
                }

                await _apiClient.Delete(SelectedItem.Id);
                Lists.Remove(SelectedItem);
                SelectedItem = null;
            }, list => SelectedItem != null); // CanExecute is based on whether SelectedItem is null
        }

        public async Task Load()
        {
            Lists.Clear();
            var lists = await _apiClient.List();
            foreach (var list in lists)
            {
                Lists.Add(list);
            }
        }
    }
}
