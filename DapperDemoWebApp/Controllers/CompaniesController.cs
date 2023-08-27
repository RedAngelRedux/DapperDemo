
using Microsoft.AspNetCore.Mvc;
using DapperDemoWebApp.Models;
using DapperDemoWebApp.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DapperDemoWebApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(_companyRepo.GetAll());
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _companyRepo == null)
            {
                return NotFound();
            }

            var company = _companyRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _companyRepo.Add(company);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    string errorMessage = string.Empty;
                    foreach (ModelError error in modelState.Errors)
                    {
                        // Get the Error details.
                        errorMessage += error.ErrorMessage.ToString();
                    }
                }
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _companyRepo == null)
            {
                return NotFound();
            }

            var company =  _companyRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _companyRepo.Update(company);
                return RedirectToAction(nameof(Index));
            }

            return View(company);

        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _companyRepo == null)
            {
                return NotFound();
            }

            _companyRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
