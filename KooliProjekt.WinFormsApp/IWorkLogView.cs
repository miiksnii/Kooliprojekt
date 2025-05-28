using KooliProjekt.PublicApi.Api;

namespace KooliProjekt.WinFormsApp
{
    public interface IWorkLogView
    {
        IList<WorkLog> TodoLists { get; set; }
        WorkLog SelectedItem { get; set; }
        string Title { get; set; }
        int Id { get; set; }
        WorkLogPresenter Presenter { get; set; }
    }
}