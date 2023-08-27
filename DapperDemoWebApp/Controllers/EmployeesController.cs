using DapperDemoWebApp.Models;
using DapperDemoWebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace DapperDemoWebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;

        [BindProperty]
        public Employee? Employee { get; set; }

        public EmployeesController(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }

        // GET:  Employees
        public async Task<IActionResult> Index()
        {
            return View(_employeeRepository.GetAll());
        }

        // GET:  Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST:  Employees/Create
        /// <summary>
        /// Handles the POST Create Action, but instead of explicitly biding to each
        /// property in the Model explicitly, we bind to the Objectr itself.  The 
        /// problem with this is that the Create signature would then be identical
        /// to the GET Create signature, hence the need for an ActionName attribute
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public IActionResult CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        // GET:  Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if( id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.GetValueOrDefault());
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }

        // POST:  Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (Employee != null && id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                _employeeRepository.Update(Employee);
                return RedirectToAction(nameof(Index));
            }

            return View(Employee);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            _employeeRepository.Remove(id.GetValueOrDefault());

            return RedirectToAction(nameof(Index)); 
        }
    }
}
