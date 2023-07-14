using MvcWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Net.Http.Headers;

namespace MvcWebApi.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string apiBaseAddress = "https://localhost:44365/";
        public async Task<ActionResult> Index()
        {
            IEnumerable<Employee> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44365/"); // Replace with your API base address

                var response = await client.GetAsync("api/Employees/Get");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(responseContent);
                }
                else
                {
                    employees = Enumerable.Empty<Employee>();
                    ModelState.AddModelError(string.Empty, "Server error. Please try again later.");
                }
            }

            return View(employees);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"api/Employees/Details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    //employee = await result.Content.ReadAsAsync<Employee>();
                    var responseContent = await result.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time."
                    );
                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Address,Company,Designation")] Employee employee)

        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBaseAddress);

                    // var response = await client.PostAsJsonAsync("Employees/Create", employee);
                    //var response = await _httpClient.PostAsync(apiBaseAddress + "api/Employees/Create", employee);
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                    _httpClient.DefaultRequestHeaders.Accept.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.PostAsync(apiBaseAddress + "api/Employees/Create", jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");

                    }
                }
            }
            return View(employee);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"api/Employees/Details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    //employee = await result.Content.ReadAsAsync<Employee>();
                    var responseContent = await result.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
        
                  
                   }
            }
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiBaseAddress);

                        var jsonContent = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                        var response = await client.PutAsync("api/Employees/Edit", jsonContent);

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error. Please try again later.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the employee. Please try again later.");
                }
            }
            return View(employee);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var result = await client.GetAsync($"api/Employees//Details/{id}");

                if (result.IsSuccessStatusCode)
                {
                    // employee = await result.Content.ReadAsAsync<Employee>();
                    var responseContent = await result.Content.ReadAsStringAsync();
                    employee = JsonConvert.DeserializeObject<Employee>(responseContent);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");


                }
            }

            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseAddress);

                var response = await client.DeleteAsync($"api/Employees/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time");


            }
            return View();
        }

    }
}






    
