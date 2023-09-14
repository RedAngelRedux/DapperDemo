using Dapper;
using DapperDemoWebApp.Models;
using Microsoft.Data.SqlClient;
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
    }
}
