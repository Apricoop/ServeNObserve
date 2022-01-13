using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ServeNObserve.Domain.Repositories;
using ServeNObserve.Domain.Services;

namespace ServeNObserve.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogger<dynamic> _logger;
        public IUserService _userService;
        public IEmployeeRepository _employeeRepository;
        public IJobRepository _jobRepository;
        public IServiceRepository _serviceRepository;
        public IJobEmployee _jobEmployeeRepository;


    }
}
