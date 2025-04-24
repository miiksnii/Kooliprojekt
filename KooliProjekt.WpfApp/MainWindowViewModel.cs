using KooliProjekt.Data;
using Kooliprojekt.Data;
using Kooliprojekt.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KooliProjekt.WpfApp
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IWorkLogService _workLogService;

        public ObservableCollection<WorkLog> WorkLogs { get; set; } = new ObservableCollection<WorkLog>();

        private WorkLog _selectedWorkLog;
        public WorkLog SelectedWorkLog
        {
            get => _selectedWorkLog;
            set => SetProperty(ref _selectedWorkLog, value, nameof(SelectedWorkLog));
        }

        public RelayCommand NewCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public MainWindowViewModel(IWorkLogService workLogService)
        {
            _workLogService = workLogService;

            // Commands
            NewCommand = new RelayCommand(NewWorkLog);
            SaveCommand = new RelayCommand(SaveWorkLog, CanSaveWorkLog);
            DeleteCommand = new RelayCommand(DeleteWorkLog, CanDeleteWorkLog);

            // Load WorkLogs
            LoadWorkLogs();
        }

        private async void LoadWorkLogs()
        {
            var result = await _workLogService.List(1, 100);
            foreach (var workLog in result.Results)
            {
                WorkLogs.Add(workLog);
            }
        }

        private void NewWorkLog()
        {
            // Logic to create a new work log (can be implemented later)
            var newWorkLog = new WorkLog { WorkerName = "New Worker", Description = "Description" };
            WorkLogs.Add(newWorkLog);
            SelectedWorkLog = newWorkLog;
        }

        private bool CanSaveWorkLog() => SelectedWorkLog != null;

        private async void SaveWorkLog()
        {
            if (SelectedWorkLog != null)
            {
                await _workLogService.Save(SelectedWorkLog);
                LoadWorkLogs(); // Reload work logs after saving
            }
        }

        private bool CanDeleteWorkLog() => SelectedWorkLog != null;

        private async void DeleteWorkLog()
        {
            if (SelectedWorkLog != null)
            {
                await _workLogService.Delete(SelectedWorkLog.Id);
                WorkLogs.Remove(SelectedWorkLog);
                SelectedWorkLog = null;
            }
        }
    }
}
