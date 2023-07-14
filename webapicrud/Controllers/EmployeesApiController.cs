using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using webapicrud.Models;



namespace webapicrud.Controllers
{
    public class EmployeesApiController : ApiController
    {
        private readonly IEmployeeRepository _employeeRepository = new EmployeeRepository();
        [HttpGet]
        [Route("api/Employees/Get")]
        public async Task<IEnumerable<Employee>> Get()
        {
            return await _employeeRepository.GetEmployees();
        }

        [HttpPost]
        [Route("api/Employees/Create")]
        public async Task CreateAsync([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.Add(employee);
            }

        }
        [HttpGet]
        [Route("api/Employees/Details/{id}")]
        public async Task<Employee> Details(int id)
        {
            var result = await _employeeRepository.GetEmployee(id);
            return result;
        }


        [HttpPut]
        [Route("api/Employees/Edit")]
        public async Task EditAsync([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepository.Update(employee);
            }
        }

        [HttpDelete]
        [Route("api/Employees/Delete/{id}")]
        public async Task DeleteConfirmedAsync(int id)
        {
            await _employeeRepository.Delete(id);
        }
    }
}
