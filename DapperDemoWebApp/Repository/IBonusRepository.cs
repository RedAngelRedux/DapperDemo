using DapperDemoWebApp.Models;
using System.ComponentModel;

namespace DapperDemoWebApp.Repository
{
    public enum SEARCH_TYPE
    {
        [Description("BEGINS WITH")]
        BEGINS_WITH = 1,
        [Description("ENDS WITH")]
        ENDS_WITH = 2,
        [Description("CONTAINS")]
        CONTAINS = 3
    };

    public interface IBonusRepository
    {
        List<Employee> GetEmployeeWithCompany();
        List<Employee> GetEmployeeWithCompany(int id);
        Company GetCompanyWithEmployees(int CompanyId);
        List<Company> GetAllCompaniesWithEmployees();
        List<Company> FilterCompaniesByName(string name, SEARCH_TYPE searchType);
        void RemoveRangeOfCompaniesWithEmployees(int[] companyId);
        void AddTestCompanyWithEmployees(Company company);
        void RemoveTestCompaniesWithEmployees();
    }
}
