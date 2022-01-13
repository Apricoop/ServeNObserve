using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.DataModels.Response;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Domain.Repositories
{
    public interface IJobRepository
    {
        Task<List<Job>> GetJobs();
        Task<Job> GetJobById(int id);
        Task<List<Job>> GetJobByTextSearch(string text);
        Task<List<Job>> GetCompletedJobs();
        Task<bool> CompleteById(int id);

        Task<JobCostResponse> GetAllOutcomeByDate(DateTime date);
        Task<int> GetAllIncomeByDate(DateTime date);

        Task<List<GeneralMoneyActivityResponse>> GetMoneyActivityByDate(DateTime? startDate = null, DateTime? endDate = null);

        Task<Job> CreateJob(Job job);
        Task<bool> DeleteJobById(int id);
        Task<bool> Save();
    }
}
