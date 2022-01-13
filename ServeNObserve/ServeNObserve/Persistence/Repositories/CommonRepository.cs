using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServeNObserve.DataModels.Response;
using ServeNObserve.Domain.Models;
using ServeNObserve.Domain.Repositories;
using ServeNObserve.Persistence.Contexts;
using ServeNObserve.Shared;

namespace ServeNObserve.Persistence.Repositories
{
    public class CommonRepository : GenericRepository<Job>, IJobRepository, IEmployeeRepository, IServiceRepository, IJobEmployee
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonRepository(DatabaseContext context, IHttpContextAccessor httpContextAccessor, ILogger<dynamic> logger) : base(context, logger)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Save()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Couldn't save changes -> {exception.Message} -- ST = {exception.StackTrace}");
                throw new ApiException(new Error { Message = "Veri tabanı güncellenirken bir hata olustu.", StackTrace = exception.StackTrace });
            }
        }

        #region Job
        public async Task<List<Job>> GetJobs()
        {
            var jobs = await _context.Jobs.ToListAsync();
            return await FillTheOutcomes(jobs);
        }

        public async Task<List<Job>> FillTheOutcomes(List<Job> jobs)
        {
            var jobEmployee = await GetJobEmployees();
            var employees = await GetEmployees();
            jobs.ForEach(j =>
            {
                var jobDay = j.EndDate.Subtract(j.StartDate).Days;
                int employeesSalary = 0;
                var employeesWorking = jobEmployee.Where(p => p.JobId == j.Id).Select(p => p.EmpoloyeeId).ToList();
                foreach (var e in employeesWorking)
                {
                    employeesSalary += employees.FirstOrDefault(p => p.Id == e).Salary;
                }
                j.EmployeeCount = employees.Count();
                j.EmployeeSalaryCost = (employeesSalary / 30 * jobDay);
            });

            return jobs;
        }

        public async Task<Job> GetJobById(int id)
        {
            var job = await _context.Jobs.Where(p => p.Id == id).FirstOrDefaultAsync();
            var jobDone = await FillTheOutcomes(new List<Job> { job });
            return jobDone.FirstOrDefault();
        }

        public async Task<List<Job>> GetJobByTextSearch(string text)
        {
            var jobs = await _context.Jobs.Where(p => p.Name.Contains(text.ToLower())).ToListAsync();
            return await FillTheOutcomes(jobs);
        }

        public async Task<List<Job>> GetCompletedJobs()
        {
            var jobs = await _context.Jobs.Where(p => p.IsCompleted).ToListAsync();
            return await FillTheOutcomes(jobs);
        }

        public async Task<bool> CompleteById(int id)
        {
            var job = await _context.Jobs.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (job == null)
                return false;

            job.IsCompleted = true;
            return await Save();
        }


        public async Task<JobCostResponse> GetAllOutcomeByDate(DateTime date)
        {
            var jobs = await GetJobs();
            jobs.ForEach(j =>
            {
                j.StartDate = Convert.ToDateTime(j.StartDate.ToString().Split(" ").First());
            });
            var matches = jobs.Where(p => p.StartDate == Convert.ToDateTime(date.ToShortDateString())).ToList();
            if (matches == null)
                return new JobCostResponse();

            var fullJobs = await FillTheOutcomes(matches);
            return new JobCostResponse
            {
                Date = date,
                GeneralCost = fullJobs.Sum(s => s.TotalCost),
                SalaryCost = fullJobs.Sum(s => s.EmployeeSalaryCost / (s.EndDate.Subtract(s.StartDate).Days == 0 ? 1 : s.EndDate.Subtract(s.StartDate).Days))
            };
        }
        public async Task<int> GetAllIncomeByDate(DateTime date)
        {
            var jobs = await GetJobs();
            jobs.ForEach(j =>
            {
                j.EndDate = Convert.ToDateTime(j.EndDate.ToString().Split(" ").First());
            });
            //var asdf = jobs.Select(s => s.ToString().Split(" ").First()).ToList();
            var matches = jobs.Where(p => p.EndDate == Convert.ToDateTime(date.ToShortDateString())).ToList();
            return matches?.Sum(s => s.Income) ?? 0;
        }
        public async Task<List<GeneralMoneyActivityResponse>> GetMoneyActivityByDate(DateTime? startDate = null, DateTime? endDate = null)
        {
            List<DateTime> incomeDates, outcomeDates;
            if (!startDate.HasValue || !endDate.HasValue)
            {
                incomeDates = await _context.Jobs.Select(s => s.EndDate).ToListAsync();
                outcomeDates = await _context.Jobs.Select(s => s.StartDate).ToListAsync();
            }
            else
            {
                incomeDates = await _context.Jobs.Select(s => s.EndDate).Where(s => s >= startDate && s <= endDate).ToListAsync();
                outcomeDates = await _context.Jobs.Select(s => s.StartDate).Where(s => s >= startDate && s <= endDate).ToListAsync();
            }

            incomeDates.AddRange(outcomeDates);
            for (int i = 0; i < incomeDates.Count; i++)
            {
                incomeDates[i] = Convert.ToDateTime(incomeDates[i].ToShortDateString());
            }
            /*
            incomeDates.ForEach(p =>
            {
                p = Convert.ToDateTime(p.ToShortDateString().Split(" ").First());
            });
            */
            var response = new List<GeneralMoneyActivityResponse>();
            var distincted = incomeDates.Distinct().ToList();

            foreach (var date in distincted)
            {
                var allIncomes = await GetAllIncomeByDate(date);
                var modelToAdd = new GeneralMoneyActivityResponse
                {
                    Date = date,
                    Income = allIncomes,
                };

                if (outcomeDates.Contains(date))
                {
                    var allOutcomes = await GetAllOutcomeByDate(date);
                    modelToAdd.Outcome = allOutcomes.Total;
                }
                response.Add(modelToAdd);
            }

            return response.OrderBy(o => o.Date).ToList();
        }

        public async Task<Job> CreateJob(Job job)
        {
            try
            {
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return job;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Yeni is veri tabanına kaydedilirken bir hata meydana geldi -> {exception.Message}");
                throw new ApiException(new Error { Message = "Yeni is veri tabanına kaydedilirken bir hata meydana geldi.", StackTrace = exception.StackTrace });
            }
        }

        public async Task<bool> DeleteJobById(int id)
        {
            ////// diger tablolarda da Joba bagli olan kayitlari sil
            var job = await _context.Jobs.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (job == null)
                return false;

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Employee
        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> GetEmployeeByTextSearch(string text)
        {
            return await _context.Employees.Where(p => p.FirstName.Contains(text.ToLower()) || p.LastName.Contains(text.ToLower())).ToListAsync();
        }

        public async Task<Employee> CreateEmployee(Employee employee)
        {
            try
            {
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return employee;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Yeni calisan veri tabanına kaydedilirken bir hata meydana geldi -> {exception.Message}");
                throw new ApiException(new Error { Message = "Yeni calisan veri tabanına kaydedilirken bir hata meydana geldi.", StackTrace = exception.StackTrace });
            }
        }

        public async Task<bool> DeleteEmployeeById(int id)
        {
            ////// diger tablolarda da Employeeye bagli olan kayitlari sil
            var employee = await _context.Employees.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee> UpdateEmployee(Employee request)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Service
        public async Task<List<Service>> GetServices()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _context.Services.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Service>> GetServiceByTextSearch(string text)
        {
            return await _context.Services.Where(p => p.Name.Contains(text.ToLower())).ToListAsync();
        }

        public async Task<Service> CreateService(Service service)
        {
            try
            {
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();
                return service;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Yeni arac veri tabanına kaydedilirken bir hata meydana geldi -> {exception.Message}");
                throw new ApiException(new Error { Message = "Yeni arac veri tabanına kaydedilirken bir hata meydana geldi.", StackTrace = exception.StackTrace });
            }
        }

        public async Task<bool> DeleteServiceById(int id)
        {
            ////// diger tablolarda da Service bagli olan kayitlari sil
            var service = await _context.Services.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (service == null)
                return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region JobEmployee
        public async Task<List<JobEmployee>> GetJobEmployees()
        {
            return await _context.JobEmployee.ToListAsync();
        }

        public async Task<JobEmployee> GetJobEmployeeById(int id)
        {
            return await _context.JobEmployee.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<JobEmployee> CreateJobEmployee(JobEmployee jobEmployee)
        {
            try
            {
                await _context.JobEmployee.AddAsync(jobEmployee);
                await _context.SaveChangesAsync();
                return jobEmployee;
            }
            catch (Exception exception)
            {
                _logger.LogInformation($"Yeni iş veri tabanına kaydedilirken bir hata meydana geldi -> {exception.Message}");
                throw new ApiException(new Error { Message = "Yeni iş veri tabanına kaydedilirken bir hata meydana geldi.", StackTrace = exception.StackTrace });
            }
        }

        public async Task<bool> DeleteJobEmployeeById(int id)
        {
            var service = await _context.JobEmployee.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (service == null)
                return false;

            _context.JobEmployee.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobEmployeeByJobId(int id)
        {
            var service = await _context.JobEmployee.Where(p => p.JobId == id).ToListAsync();
            if (service == null)
                return false;

            foreach(var s in service)
                _context.JobEmployee.Remove(s);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteJobEmployeeByEmployeeId(int id)
        {
            var service = await _context.JobEmployee.Where(p => p.EmpoloyeeId == id).ToListAsync();
            if (service == null)
                return false;

            foreach (var s in service)
                _context.JobEmployee.Remove(s);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
