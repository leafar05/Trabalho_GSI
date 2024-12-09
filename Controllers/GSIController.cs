using GSI.WebApi.Responses;
using GSI_WebApi.AD;
using GSI_WebApi.DB;
using Microsoft.AspNetCore.Mvc;

namespace GSI.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GSIController : ControllerBase
    {
        public GSIController()
        {
        }

        [HttpPost(Name = nameof(SyncDbFromAd))]
        public void SyncDbFromAd()
        {
            using (var db = new MainDbContext())
            {
                var users = AdHelper.GetUsers();

                foreach (var user in users)
                {
                    var employee = db.Employees.FirstOrDefault(x => x.Guid == user.Id);
                    if (employee == null)
                    {
                        employee = new Employee();
                        employee.Guid = user.Id;
                        db.Employees.Add(employee);
                    }
                    employee.Name = user.Name;
                    employee.ExpirationDate = user.AccountExpires;
                    employee.IsEnabled = user.IsEnabled;
                    employee.CreationDate = user.CreationDate;
                    employee.LastPasswordSet = user.LastPasswordChange;
                    employee.LoginCount = user.NLogins;
                    employee.department = user.Department;
                }

                db.SaveChanges();
            }
        }

        [HttpGet(Name = nameof(GetEmployeesFromBd))]
        public IActionResult GetEmployeesFromBd()
        {
            using (var db = new MainDbContext())
            {
                var employees = db.Employees.ToList();
                return Ok(employees);
            }
        }

        [HttpGet(Name = nameof(GetUsersByDepartment))]
        public IEnumerable<Employee> GetUsersByDepartment([FromQuery] string department)
        {
            using (var db = new MainDbContext())
            {
                var employees = db.Employees
                                  .Where(x => x.department == department).ToList();
                return employees;
            }
        }

        [HttpGet(Name = nameof(GetDisabledUsers))]
        public IEnumerable<Employee> GetDisabledUsers()
        {
            using (var db = new MainDbContext())
            {
                var employees = db.Employees.Where(x => !x.IsEnabled).ToList();
                return employees;
            }
        }

        [HttpGet(Name = nameof(GetActiveUsersWithLogins))]
        public IEnumerable<Employee> GetActiveUsersWithLogins()
        {
            using (var db = new MainDbContext())
            {
                var employees = db.Employees.Where(x => x.IsEnabled && x.LoginCount >= 2).ToList();
                return employees;
            }
        }

        [HttpGet(Name = nameof(GetUsersWithExpiringPasswords))]
        public IEnumerable<Employee> GetUsersWithExpiringPasswords()
        {
            using (var db = new MainDbContext())
            {
                var expiringDate = DateTime.Now.AddDays(7);
                var employees = db.Employees.Where(x => x.ExpirationDate.Value.AddDays(-7) < DateTime.Now).ToList();
                return employees;
            }
        }
    }
}
