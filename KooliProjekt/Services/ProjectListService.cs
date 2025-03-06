using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProjectListService : IProjectListService
    {
<<<<<<< HEAD
        private readonly IProjectListRepository _projectListRepository;


        public ProjectListService(IUnitOfWork unitOfWork, IProjectListRepository projectListRepository)
=======
        private readonly ApplicationDbContext _ProjectListService;
        private IProjectListService @object;

        public ProjectListService(ApplicationDbContext context)
>>>>>>> 70b27eba397d84857bbd0cb5e4abd84079f159fe
        {
            _projectListRepository = projectListRepository;
        }

<<<<<<< HEAD
        public async Task Delete(int id)
=======
        public ProjectListService(IProjectListService @object)
        {
            this.@object = @object;
        }

        public async Task Delete(int? id)
>>>>>>> 70b27eba397d84857bbd0cb5e4abd84079f159fe
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
