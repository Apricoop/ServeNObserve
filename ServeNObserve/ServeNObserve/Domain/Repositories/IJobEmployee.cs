using ServeNObserve.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServeNObserve.Domain.Repositories
{
    public interface IJobEmployee
    {
        Task<List<JobEmployee>> GetJobEmployees();
        Task<JobEmployee> GetJobEmployeeById(int id);
        Task<JobEmployee> CreateJobEmployee(JobEmployee jobEmployee);
        Task<bool> DeleteJobEmployeeById(int id);
        Task<bool> DeleteJobEmployeeByJobId(int id);
        Task<bool> DeleteJobEmployeeByEmployeeId(int id);
        Task<bool> Save();
    }
}
