using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProjectItemService : IProjectItemService
    {
        private readonly IProjectItemRepository _projectItemRepository;

        public ProjectItemService(IUnitOfWork unitOfWork, IProjectItemRepository projectItemRepository)
        {
            _projectItemRepository = projectItemRepository;
        }

        public async Task Delete(int id)
        {
            await _projectItemRepository.Delete(id); // Use the repository directly
        }

        public async Task<ProjectItem> Get(int id)
        {
            return await _projectItemRepository.Get(id); // Use the repository directly
        }

        public async Task<PagedResult<ProjectItem>> List(int page, int pageSize, ProjectItemSearch search = null)
        {
            return await _projectItemRepository.List(page, pageSize); // Use the repository directly
        }

        public async Task Save(ProjectItem item)
        {
            // Pass the saving logic to the repository layer
            await _projectItemRepository.Save(item);  // No need for direct context handling here
        }

        public bool ProjectItemExists(int id)
        {
            // Use the Get method to check if the ProjectItem exists
            var projectItem = _projectItemRepository.Get(id).Result; // Synchronously wait for the result
            return projectItem != null;
        }
    }

}
