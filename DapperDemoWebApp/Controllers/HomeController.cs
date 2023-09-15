using DapperDemoWebApp.Models;
using DapperDemoWebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Faker;

namespace DapperDemoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonusRepository;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepository)
        {
            _logger = logger;
            _bonusRepository = bonusRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Models.Company> companies = _bonusRepository.GetAllCompaniesWithEmployees();
            return View(companies);
        }

        public IActionResult AddTestRecords()
        {
            Models.Company company = new Models.Company()
            {
                //Name = "Test" + Guid.NewGuid().ToString(),
                Name = " " + Faker.Company.Name(),
                Address = Faker.Address.StreetAddress(),
                City = Faker.Address.City(),
                PostalCode = Faker.Address.ZipCode(),
                State = Faker.Address.UsStateAbbr(),
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Phone = Faker.Phone.Number(),
                Title = GenerateRandomJobTitle()
            });

            company.Employees.Add(new Employee()
            {
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Phone = Faker.Phone.Number(),
                Title = GenerateRandomJobTitle()
            });

            _bonusRepository.AddTestCompanyWithEmployees(company);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveTestRecords()
        {
            _bonusRepository.RemoveTestCompaniesWithEmployees();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Random String Generators - Consider Creating a Utiliy Class with These
        private static string GenerateRandomJobTitle()
        {
            Random random = new Random();
            string[] adjectives = { "IT ", "Sales ", "Customer Service ", "HR ", "Marketing " };
            string[] nouns = { "Director", "Consultant", "Operations", "Manager", "Advocate", "Technician", "Vice-president" };
            string companyName = adjectives[random.Next(adjectives.Length)] + nouns[random.Next(nouns.Length)];
            return companyName;
        }
        //public static string GenerateRandomName()
        //{
        //    Random random = new Random();
            //string[] maleFirstNames = { "Liam", "Noah", "Oliver", "Elijah", "William", "James", "Benjamin", "Lucas", "Henry", "Alexander" };
            //string[] femaleFirstNames = { "Emma", "Olivia", "Ava", "Isabella", "Sophia", "Mia", "Charlotte", "Amelia", "Harper", "Evelyn" };
            //string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };
            //string firstName, lastName;

        //    if (random.Next(2) == 0) // 0 for male, 1 for female
        //        firstName = Faker.Name.First();
        //        firstName = maleFirstNames[random.Next(maleFirstNames.Length)];
        //    else
        //        firstName = femaleFirstNames[random.Next(femaleFirstNames.Length)];

        //    lastName = lastNames[random.Next(lastNames.Length)];

        //    return firstName + ' ' + lastName;
        //}

    }
}