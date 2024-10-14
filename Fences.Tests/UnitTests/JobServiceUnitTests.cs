using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.Interfaces;
using Fences.Tests.UnitTests;
using Fences.ViewModels.VM;
using Microsoft.EntityFrameworkCore;

public class JobServiceUnitTests : BaseUnitTests
{
    private readonly IJobService _jobService;

    public JobServiceUnitTests(IJobService jobService, ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _jobService = jobService;
    }

    [Fact]
    public async Task GetJobAsync()
    {
        var job = await _jobService.GetJobAsync(x => x.Town == "Opole");
        Assert.NotNull(job);
    }

    [Fact]
    public async Task GetJobsAsync()
    {
        var jobs = await _jobService.GetJobsAsync(x => x.Id > 2 && x.Id <= 4);

        Assert.NotNull(jobs);
        Assert.NotEmpty(jobs);
        Assert.Equal(2, jobs.Count());
    }

    [Fact]
    public async Task GetAllJobsAsync()
    {
        var jobs = await _jobService.GetJobsAsync();
        Assert.NotNull(jobs);
        Assert.NotEmpty(jobs);
        Assert.Equal(DbContext.Jobs.Count(), jobs.Count());
    }

    [Fact]
    public async Task AddJobAsync_ShouldAddJob_WhenValidData()
    {
        var addJobVm = new AddJobVm
        {
            Town = "Poznan",
            ZipCode = "60-001",
            TotalLength = 100,
            JobType = "Fencing",
            Height = 2.0
        };
        var job = new Job
        {
            Id = 1,
            Town = addJobVm.Town,
            ZipCode = addJobVm.ZipCode,
            TotalLength = addJobVm.TotalLength,
            JobType = addJobVm.JobType,
            Height = addJobVm.Height,
            RegistrationDate = DateTime.Now,
            DateOfExecution = DateTime.Now.AddMonths(3)
        };

        await _jobService.AddJobAsync(addJobVm);
        var addedJob = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Town == job.Town);

        Assert.NotNull(addedJob);
        Assert.Equal(addJobVm.Town, addedJob.Town);
    }

    [Fact]
    public async Task UpdateJobAsync_ShouldUpdateJob_WhenJobExists()
    {
        var job = new Job
        {
            Id = 1,
            Town = "Old Town",
            ZipCode = "00-001",
            JobType = "Construction",
            TotalLength = 120,
            Height = 2.5,
            RegistrationDate = DateTime.Now,
            DateOfExecution = DateTime.Now.AddMonths(2)
        };
        await DbContext.Jobs.AddAsync(job);
        await DbContext.SaveChangesAsync();

        var updateJobVm = new UpdateJobVm
        {
            Id = job.Id,
            Town = "New Town",
            ZipCode = "00-002",
            JobType = "Renovation",
            TotalLength = 150,
            Height = 3.0
        };

        await _jobService.UpdateJobAsync(updateJobVm);
        var updatedJob = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Id == job.Id);

        Assert.NotNull(updatedJob);
        Assert.Equal(updateJobVm.Town, updatedJob.Town);
        Assert.Equal(updateJobVm.ZipCode, updatedJob.ZipCode);
        Assert.Equal(updateJobVm.JobType, updatedJob.JobType);
    }

    [Fact]
    public async Task DeleteJobAsync_ShouldRemoveJob_WhenJobExists()
    {
        var job = new Job
        {
            Id = 1,
            Town = "Warsaw",
            ZipCode = "00-001",
            JobType = "Demolition",
            TotalLength = 100,
            Height = 2.5,
            RegistrationDate = DateTime.Now,
            DateOfExecution = DateTime.Now.AddMonths(1)
        };
        await DbContext.Jobs.AddAsync(job);
        await DbContext.SaveChangesAsync();

        var jobVm = new JobVm { Id = job.Id };

        await _jobService.DeleteJobAsync(jobVm);
        var deletedJob = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Id == job.Id);

        Assert.Null(deletedJob);
    }
}
