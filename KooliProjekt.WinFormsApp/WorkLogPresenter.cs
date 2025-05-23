using KooliProjekt.WinFormsApp.Api;

namespace KooliProjekt.WinFormsApp
{
    public class WorkLogPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IWorkLogView _todoView;

        public WorkLogPresenter(IWorkLogView todoView, IApiClient apiClient)
        {
            _apiClient = apiClient;
            _todoView = todoView;

            todoView.Presenter = this;
        }

        public void UpdateView(IWorkLogView list)
        {
            if (list == null)
            {
                _todoView.Title = string.Empty;
                _todoView.Id = 0;
            }
            else
            {
                _todoView.Id = list.Id;
                _todoView.Title = list.Title;

            }
        }


        public async Task Load()
        {
            var todoLists = await _apiClient.List();

            _todoView.TodoLists = todoLists.Value;
        }
    }
}