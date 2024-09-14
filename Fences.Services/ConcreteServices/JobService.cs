using AutoMapper;
using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Fences.Services.ConcreteServices
{
    public class JobService : BaseService, IJobService
    {
        public JobService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) : base(dbContext, mapper, logger)
        {
        }

        public async Task AddJobAsync(AddJobVm addJobVm)
        {
            try
            {
                if (addJobVm == null)
                    throw new ArgumentNullException($"View model parameter is null");
                var jobEntity = Mapper.Map<Job>(addJobVm);
                await DbContext.Jobs.AddAsync(jobEntity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateJobAsync(UpdateJobVm updateJobVm)
        {
            try
            {
                if(updateJobVm == null)
                    throw new ArgumentNullException("View model parameter is null");
                var jobEntity = Mapper.Map<Job>(updateJobVm);
                DbContext.Jobs.Update(jobEntity);
                await DbContext.SaveChangesAsync();
                var jobVm = Mapper.Map<JobVm>(jobEntity);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<JobVm> GetJobAsync(Expression<Func<Job, bool>>? filterExpression = null)
        {
            try
            {
                if (filterExpression == null)
                    throw new ArgumentNullException($"Filter expression is null");
                var jobEntity = await DbContext.Jobs.FirstOrDefaultAsync(filterExpression);
                var jobVm = Mapper.Map<JobVm>(jobEntity);
                return jobVm;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<JobVm>> GetJobsAsync(Expression<Func<Job, bool>>? filterExpression = null)
        {
            try
            {
                var jobEntities = filterExpression == null
                    ? await DbContext.Jobs.OrderBy(j => j.DateOfExecution).ToListAsync()
                    : await DbContext.Jobs.Where(filterExpression).OrderBy(j => j.DateOfExecution).ToListAsync();
                var jobVms = Mapper.Map<IEnumerable<JobVm>>(jobEntities);
                return jobVms;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task DeleteJobAsync(JobVm jobVm)
        {
            try
            {
                var jobEntity = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobVm.Id);
                if (jobEntity == null)
                {
                    throw new ArgumentException($"Job with Id {jobVm.Id} not found");
                }
                DbContext.Jobs.Remove(jobEntity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
