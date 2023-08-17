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
            throw new NotImplementedException();
        }

        public Company Find(int id)
        {
            throw new NotImplementedException();
        }

        public List<Company> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public Company Update(Company company)
        {
            throw new NotImplementedException();
        }
    }
}
