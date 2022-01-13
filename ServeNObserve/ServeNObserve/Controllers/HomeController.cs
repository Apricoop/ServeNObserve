using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServeNObserve.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.DataModels.Request;
using ServeNObserve.Domain.Models;
using ServeNObserve.Domain.Repositories;
using ServeNObserve.Domain.Services;

namespace ServeNObserve.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(IUserService userService, IEmployeeRepository employeeRepo, IJobRepository jobRepo, IServiceRepository serviceRepo, ILogger<HomeController> logger, IJobEmployee jobEmployee)
        {
            _userService = userService;
            _employeeRepository = employeeRepo;
            _jobRepository = jobRepo;
            _serviceRepository = serviceRepo;
            _logger = logger;
            _jobEmployeeRepository = jobEmployee;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _jobRepository.GetJobs();
            ViewBag.Jobs = jobs;
            ViewData["Jobs"] = jobs;
            return View();
        }
        public async Task<IActionResult> AddService(Service service)
        {
            await _serviceRepository.CreateService(service);
            return RedirectToAction("Tables");
        }
        public async Task<IActionResult> DeleteService(int id)
        {
            if(!(await _employeeRepository.GetEmployees()).Any(e=>e.CarId == id))
                await _serviceRepository.DeleteServiceById(id);
            return RedirectToAction("Tables");
        }
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
                if(employee.IsCorrect())
                    await _employeeRepository.CreateEmployee(employee);
            }
            catch (Shared.ApiException ex)
            {
                Console.WriteLine(ex.Error.ToString());
            }
            return RedirectToAction("Tables");
        }
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployeeById(id);
            await _jobEmployeeRepository.DeleteJobEmployeeByEmployeeId(id);
            return RedirectToAction("Tables");
        }
        public async Task<IActionResult> AddJob(Job job, int[] employeeIds)
        {
            job.StartDate = Convert.ToDateTime(job.StartDate.ToShortDateString());
            job.EndDate = Convert.ToDateTime(job.EndDate.ToShortDateString());

            try
            {
                if (job.IsCorrect())
                {
                    await _jobRepository.CreateJob(job);
                    foreach(var i in employeeIds)
                    {
                        var employee = await _employeeRepository.GetEmployeeById(i);
                        if (employee != null)
                            await _jobEmployeeRepository.CreateJobEmployee(new JobEmployee()
                            {
                                EmpoloyeeId = employee.Id,
                                JobId = job.Id
                            });
                    }
                }
            }
            catch (Shared.ApiException ex)
            {
                Console.WriteLine(ex.Error.ToString());
            }
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<Job> GetJob(int id)
        {
            var job = await _jobRepository.GetJobById(id);
            return job;
        }
        [HttpGet]
        public async Task<List<int>> GetEmployeeIdsWorkingOnJob(int jobId)
        {
            var jobEmployee = await _jobEmployeeRepository.GetJobEmployees();
            return jobEmployee.Where(j=>j.JobId == jobId).Select(j=>j.EmpoloyeeId).ToList();
        }
        [HttpGet]
        public async Task<List<Employee>> GetEmployees()
        {
            var e = await _employeeRepository.GetEmployees();
            return e;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteJob(int id)
        {
            await _jobRepository.DeleteJobById(id);
            await _jobEmployeeRepository.DeleteJobEmployeeByJobId(id);
            return RedirectToAction("Dashboard");
        }
        public async Task<IActionResult> GetBalanceBetweenDate(string startDate, string endDate)
        {
            var s = Convert.ToDateTime(startDate);
            var e = Convert.ToDateTime(endDate);

            var balanceRange = await _jobRepository.GetMoneyActivityByDate(s,e);
            TempData.Put("BalanceRange", balanceRange);
            return RedirectToAction("Dashboard");
        }
        public async Task<IActionResult> Dashboard()
        {
            ViewData["Jobs"] = await _jobRepository.GetJobs();
            //var mock1 = await _jobRepository.GetAllIncomeByDate(DateTime.Now.AddDays(8));
            //var mock2 = await _jobRepository.GetAllOutcomeByDate(DateTime.Now.AddDays(8));

            var moneyActivities = await _jobRepository.GetMoneyActivityByDate();
            ViewData["MoneyActivities"] = moneyActivities;
            return View();
        }
        [HttpGet]
        public async Task<List<DataModels.Response.GeneralMoneyActivityResponse>> TestMethod()
        {
            var moneyActivities = await _jobRepository.GetMoneyActivityByDate();
            return moneyActivities;
        }
        public async Task<IActionResult> Tables()
        {
            ViewData["Employees"] = await _employeeRepository.GetEmployees();
            ViewData["Services"] = await _serviceRepository.GetServices();
            return View();
        }

        // Views altina bu controller adinda bir class olusturmak gerek (Home)
        // O dosya altinda bu method adiyla (Privacy) cshtml dosyasi olusturuyoruz ve return View o cshtml dosyasina modeli return ediyor.
        public async Task<IActionResult> Privacy()
        {
            // bu modeli html tarafindan alip ona gore islem yapmak gerekecek
            var model = new Job
            {
                Electricity = 70,
                Gas = 70,
                Income = 2000,
                Name = "Job - 1",
                Other = 100,
                StartDate = Convert.ToDateTime(DateTime.Now.AddDays(9).ToShortDateString()),
                Water = 40,
                EndDate = Convert.ToDateTime(DateTime.Now.AddDays(10).ToShortDateString()),
            };
            var result = await _jobRepository.CreateJob(model);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
