using DapperDemoWebApp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DapperDemoWebApp.Repository
{
    public interface IBonusRepository
    {
        public List<Employee> GetEmployeeWithCompany();
        public List<Employee> GetEmployeeWithCompany(int id);
        public Company GetCompanyWithEmployees(int CompanyId);

    }
}
