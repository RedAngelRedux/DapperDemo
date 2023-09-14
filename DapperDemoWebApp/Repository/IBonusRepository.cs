using DapperDemoWebApp.Models;

namespace DapperDemoWebApp.Repository
{
    public interface IBonusRepository
    {
        public List<Employee> GetEmployeeWithCompany();
    }
}
