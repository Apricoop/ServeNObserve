using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<List<Employee>> GetEmployeeByTextSearch(string text);

        Task<Employee> CreateEmployee(Employee employee);
        Task<bool> DeleteEmployeeById(int id);

        Task<Employee> UpdateEmployee(Employee request);
    }
}
