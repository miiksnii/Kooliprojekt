using KooliProjekt.PublicApi;
using KooliProjekt.PublicApi.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form, IWorkLogView
    {
        public IList<ApiWorkLog> WorkLogs
        {
            get => (IList<ApiWorkLog>)TodoListsGrid.DataSource;
            set => TodoListsGrid.DataSource = value;
        }

        public ApiWorkLog SelectedItem { get; set; }

        public WorkLogPresenter Presenter { get; set; }

        public int Id
        {
            get => int.TryParse(IdField.Text, out int id) ? id : 0;
            set => IdField.Text = value.ToString();
        }

        public DateTime Date
        {
            get => DateTime.TryParse(DateField.Text, out DateTime date) ? date : DateTime.MinValue;
            set => DateField.Text = value.ToString("yyyy-MM-dd");
        }

        public int TimeSpent
        {
            get => int.TryParse(TimeSpentField.Text, out int time) ? time : 0;
            set => TimeSpentField.Text = value.ToString();
        }

        public string WorkerName
        {
            get => WorkerNameField.Text;
            set => WorkerNameField.Text = value;
        }

        public string Description
        {
            get => DescriptionField.Text;
            set => DescriptionField.Text = value;
        }

        public Form1()
        {
            InitializeComponent();

            var apiClient = new ApiClient();
            Presenter = new WorkLogPresenter(apiClient, this);

            TodoListsGrid.SelectionChanged += TodoListsGrid_SelectionChanged;
            NewButton.Click += NewButton_Click;
            SaveButton.Click += async (s, e) => await Presenter.Save();
            DeleteButton.Click += async (s, e) => await Presenter.Delete();

            Load += async (s, e) => await Presenter.Load();
        }

        private void TodoListsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (TodoListsGrid.SelectedRows.Count > 0)
            {
                var selected = TodoListsGrid.SelectedRows[0].DataBoundItem as ApiWorkLog;
                SelectedItem = selected;
                Presenter.UpdateView(selected);
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            Presenter.New();
        }
    }
}
