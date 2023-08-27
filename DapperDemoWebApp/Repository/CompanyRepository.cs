using Dapper;
using DapperDemoWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemoWebApp.Repository
{
    public class CompanyRepository : ICompanyRepository
    {

        private IDbConnection db;

        public CompanyRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }

        public Company Add(Company company)
        {
            var sql = 
                "INSERT INTO Companies (Name, Address, City, State, PostalCode) VALUES(@Name, @Address, @City, @State, @PostalCode);"
                + " SELECT CAST(SCOPE_IDENTITY() as int);";

            // Further, because of the consisten naming, the above can be further simplified as...
            var id = db.Query<int>(sql,company).Single();


            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            var sql = "SELECT [CompanyId], [Name], [Address], [City], [State], [PostalCode] FROM Companies WHERE [CompanyId] = @CompanyId";
            return db.Query<Company>(sql, new { @CompanyId = id }).Single();
        }

        public List<Company> GetAll()
        {
            var sql = "SELECT [CompanyId], [Name], [Address], [City], [State], [PostalCode] FROM Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            var sql = "DELETE FROM Companies WHERE CompanyId = @Id";
            db.Execute(sql, new {id });
        }

        public Company Update(Company company)
        {
            var sql = 
                "UPDATE Companies SET Name = @Name, Address = @Address, "
                + "City = @City, State = @State, PostalCode = @PostalCode "
                + "WHERE CompanyId = @CompanyId\r\n";

            db.Execute(sql, company);

            return company;
        }
    }
}
