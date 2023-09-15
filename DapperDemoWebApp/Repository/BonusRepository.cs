using Dapper;
using DapperDemoWebApp.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace DapperDemoWebApp.Repository
{
    public class BonusRepository : IBonusRepository
    {
        private IDbConnection _db;
        public BonusRepository(IConfiguration configuration) 
        {
            this._db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public void AddTestCompanyWithEmployees(Company company)
        {
            var sqlCompany =
                "INSERT INTO [Companies] ([Name], [Address], [City], [State], [PostalCode]) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                + " SELECT CAST(SCOPE_IDENTITY() as int);";

            // Further, because of the consisten naming, the above can be further simplified as...
            var companyId = _db.Query<int>(sqlCompany, company).Single();
            company.CompanyId = companyId;

            foreach(var employee in company.Employees)
            {
                employee.CompanyId = company.CompanyId;
                var sqlEmployee =
                    "INSERT INTO [Employees] ([Name], [Title], [Email], [Phone], [CompanyId]) VALUES(@Name, @Title, @Email, @Phone, @CompanyId);"
                    + " SELECT CAST(SCOPE_IDENTITY() as int);";

                // Further, because of the consisten naming, the above can be further simplified as...
                _db.Query<int>(sqlEmployee, employee).Single();    
            }
        }

        public List<Company> FilterCompaniesByName(string snippet,SEARCH_TYPE searchType)
        {
            var sql = "SELECT * FROM Companies WHERE [Name] ";
            string condition = "";

            switch (searchType)
            {
                case SEARCH_TYPE.BEGINS_WITH:
                    condition = $"LIKE '{snippet}%'";
                    break;
                case SEARCH_TYPE.ENDS_WITH:
                    condition = $"LIKE '%{snippet}'";
                    break;
                case SEARCH_TYPE.CONTAINS:
                    condition = $"LIKE '%{snippet}'";
                    break;
            }
            sql += condition;

            return _db.Query<Company>(sql).ToList();
        }

        public List<Company> GetAllCompaniesWithEmployees()
        {
            var sql = "SELECT " +
                "c.[CompanyId], c.[Name], c.[Address], c.[City], c.[State], c.[PostalCode],e.[EmployeeId], " +
                "e.[Name], e.[Email], e.[Phone], e.[Title], e.[CompanyId] " +
                "FROM [Companies] c LEFT OUTER JOIN [Employees] e ON c.CompanyId = e.CompanyId";

            var companyDictionary = new Dictionary<int, Company>();

            var company = _db.Query<Company, Employee, Company>(sql, (c, e) =>
            {
                if (!companyDictionary.TryGetValue(c.CompanyId, out var currentCompany))
                {
                    currentCompany = c;
                    companyDictionary.Add(currentCompany.CompanyId, currentCompany);
                }
                currentCompany.Employees.Add(e);
                return currentCompany;

            }, splitOn: "EmployeeId");
            
            return company.Distinct().ToList();
        }

        public Company GetCompanyWithEmployees(int CompanyId)
        {
            var p = new { CompanyId = CompanyId };

            var sql = "SELECT * FROM Companies WHERE CompanyId = @CompanyId;"
                + "SELECT * FROM Employees WHERE CompanyId = @CompanyId;";

            Company company;

            using (var lists = _db.QueryMultiple(sql, p))
            {
                company = lists.Read<Company>().ToList().FirstOrDefault();
                company.Employees = lists.Read<Employee>().ToList();
            }

            return company;
        }

        public List<Employee> GetEmployeeWithCompany()
        {
            var sql = "SELECT e.[EmployeeId], e.[Name], e.[Email], e.[Phone], " +
                "e.[Title], e.[CompanyId], c.[CompanyId], c.[Name], c.[Address], " +
                "c.[City], c.[State], c.[PostalCode] FROM [Employees] AS e " +
                "INNER JOIN [Companies] AS c ON e.[CompanyId] = c.[CompanyId]";

            var employee = _db.Query<Employee, Company, Employee>(sql, (e,c) => {
                e.Company = c;
                return e;
            }, splitOn: "CompanyId");

            return employee.ToList();
        }

        public List<Employee> GetEmployeeWithCompany(int id = 0)
        {
            var sql = "SELECT e.[EmployeeId], e.[Name], e.[Email], e.[Phone], " +
                "e.[Title], e.[CompanyId], c.[CompanyId], c.[Name], c.[Address], " +
                "c.[City], c.[State], c.[PostalCode] FROM [Employees] AS e " +
                "INNER JOIN [Companies] AS c ON e.[CompanyId] = c.[CompanyId]";

            if(id != 0)
            {
                sql += " WHERE e.CompanyId = @Id ";
            }

            var employee = _db.Query<Employee, Company, Employee>(sql, (e, c) => {
                e.Company = c;
                return e;
            },new { id=id }, splitOn: "CompanyId");

            return employee.ToList();
        }

        public void RemoveRangeOfCompaniesWithEmployees(int[] companyId)
        {
            // Because of the FOREIGH KEY constraint, you must first delete the employees, then the company
            foreach( var id in companyId)
            {
                _db.Query("DELETE FROM [Employees] WHERE [CompanyId] = @companyId", new { companyId = id });
            }
            _db.Query("DELETE FROM [Companies] WHERE [CompanyId] in @companyId", new { companyId });
        }

        public void RemoveTestCompaniesWithEmployees()
        {
            int[] companiesToDelete = FilterCompaniesByName(" ", SEARCH_TYPE.BEGINS_WITH).Select(i => i.CompanyId).ToArray();
            RemoveRangeOfCompaniesWithEmployees(companiesToDelete);
        }
    }
}
