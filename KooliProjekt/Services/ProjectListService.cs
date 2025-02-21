using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProjectListService : IProjectListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectListRepository _projectListRepository;


        public ProjectListService(IUnitOfWork unitOfWork, IProjectListRepository projectListRepository)
        {
            _unitOfWork = unitOfWork;
            _projectListRepository = projectListRepository;
        }

        public async Task Delete(int id)
        {
            await _projectListRepository.Delete(id); // Use the repository directly
        }

        public async Task<ProjectList> Get(int id)
        {
            return await _projectListRepository.Get(id); // Use the repository directly
        }

        public async Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null)
        {
            return await _projectListRepository.List(page, pageSize); // Use the repository directly
        }

        public async Task Save(ProjectList list)
        {
            await _projectListRepository.Save(list); // Use the repository directly
        }

        public bool ProjectListExists(int id)
        {
            // Use the Get method to check if the ProjectList exists
            var projectList = _projectListRepository.Get(id).Result; // Synchronously wait for the result
            return projectList != null;
        }
    }
}
