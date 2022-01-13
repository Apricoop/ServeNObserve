using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServeNObserve.Domain.Models;

namespace ServeNObserve.Domain.Repositories
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServices();
        Task<Service> GetServiceById(int id);
        Task<List<Service>> GetServiceByTextSearch(string text);

        Task<Service> CreateService(Service service);
        Task<bool> DeleteServiceById(int id);
    }
}
