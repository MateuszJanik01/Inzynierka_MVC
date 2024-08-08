using AutoMapper;
using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fences.Services.ConcreteServices
{
    public class JobService : BaseService, IJobService
    {
        public JobService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public JobVm AddOrUpdateJob(AddOrUpdateJobVm addOrUpdateJobVm)
        {
            try
            {
                if (addOrUpdateJobVm == null)
                    throw new ArgumentNullException($"View model parameter is null");
                var jobEntity = Mapper.Map<Job>(addOrUpdateJobVm);
                if (!addOrUpdateJobVm.Id.HasValue || addOrUpdateJobVm.Id == 0)
                    DbContext.Jobs.Add(jobEntity);
                else
                    DbContext.Jobs.Update(jobEntity);
                DbContext.SaveChanges();
                var jobVm = Mapper.Map<JobVm>(jobEntity);
                return jobVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public JobVm GetJob(Expression<Func<Job, bool>>? filterExpression = null)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException($"Filter expression is null");
                var jobEntity = DbContext.Jobs.FirstOrDefault(filterExpression);
                var jobVm = Mapper.Map<JobVm>(jobEntity);
                return jobVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<JobVm> GetJobs(Expression<Func<Job, bool>>? filterExpression = null)
        {
            try
            {
                var jobEntities = DbContext.Jobs.AsQueryable();
                if (filterExpression != null)
                    jobEntities = jobEntities.Where(filterExpression);
                var jobVms = Mapper.Map<IEnumerable<JobVm>>(jobEntities);
                return jobVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
