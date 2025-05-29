using KooliProjekt.PublicApi.Api;

namespace KooliProjekt.WinFormsApp
{
    public interface IWorkLogView
    {
        IList<ApiWorkLog> TodoLists { get; set; }
        ApiWorkLog SelectedItem { get; set; }
        string Title { get; set; }
        int Id { get; set; }
        WorkLogPresenter Presenter { get; set; }
    }
}