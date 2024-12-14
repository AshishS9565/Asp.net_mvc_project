using Asp.Net_Web_Apis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp.Net_Web_Apis.Repository
{
    public interface IRepository
    {
        // List<EmployeeModel> GetEmployees(string filter = null, int pageNumber = 1, int pageSize = 10); // Ensure this returns List<EmployeeModel>
        List<EmployeeModel> GetEmployees();
        string AddEmployee(EmployeeModel employee);
        string UpdateEmployee(int ID , EmployeeModel employee);
        string DeleteEmployee(int id);
       string  BulkInsertEmployees(List<EmployeeModel> employees); // New method for bulk insert
        List<AllCities> GetAllCities();

        List<AllCountries> GetAllCountries();
       // string AddEmployee(EmployeeModel employee);
        string UpdateEmployee(EmployeeModel employee);
       // string DeleteEmployee(int id);

        EmployeeModel VerifyLogin(string Email, string Password);  // New method to verify login
        string AddEmployeeFromExcel(string Name, string Email, int CityID, string Gender, bool Skills, string Hobbies, string CityName, string CountryIDs);
    }
    
}
