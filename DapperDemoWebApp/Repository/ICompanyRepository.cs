using DapperDemoWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace DapperDemoWebApp.Repository
{
    public interface ICompanyRepository
    {
        Company? Find(int id);
        List<Company> GetAll();
        Company Add(Company company);
        Company Update(Company company);
        void Remove(int id);
    }
}
