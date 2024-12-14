using Asp.Net_Web_Apis.Models;
using Asp.Net_Web_Apis.Repository;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Asp.Net_Web_Apis.Controllers
{
    public class EmployeeController:Controller
    {
        private IRepository _repository;

        public EmployeeController(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        //ctr+dot for generate constructor

        public ActionResult List()
        {
            return View();

        }
        [HttpGet]
        public ActionResult GetEmployee ()//This declares the GetEmployee method, which does not take any parameters. The method will return an ActionResult.
        {
            List<EmployeeModel> list = _repository.GetEmployees(); //Fetching Employee List:The method retrieves the list of employees from the database by calling _repository.GetEmployees().

            string json_data = JsonConvert.SerializeObject(list); //The list of employees is converted to a JSON string using 

            return Json(json_data, JsonRequestBehavior.AllowGet); //The method returns the JSON string as an ActionResult.
            //var employees = _repository.GetEmployees(filter, pageSize, pageNumber);
            //return Json(employees, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetAllCities()
        {
            List<AllCities> list = _repository.GetAllCities();

            string json_data = JsonConvert.SerializeObject(list);

            return Json(json_data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAllCountries()
        {
            List<AllCountries> list = _repository.GetAllCountries();

            string json_data = JsonConvert.SerializeObject(list);

            return Json(json_data, JsonRequestBehavior.AllowGet);
        }


        // This method saves image file to the server
        private string SaveEmployeeImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("/Content/ProfileImage"), fileName);
                file.SaveAs(path);
                return "/Content/ProfileImage/" + fileName;
            }
            return null;
        }


        [HttpPost]
        public ActionResult AddEmployee(EmployeeModel empDetails)  //This declares the AddEmployee method which takes an EmployeeModel object named empDetails as a parameter. This parameter will contain the employee details sent from the client-side (usually via an AJAX call).
        {
            // Use the helper method to save the file
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                empDetails.ImagePath = SaveEmployeeImage(file);
            }
            string result = _repository.AddEmployee(empDetails); //The method calls _repository.AddEmployee(empDetails) to add the new employee details to the database.

            List<EmployeeModel> list = _repository.GetEmployees(); //Fetching Updated Employee List:-->>This method returns a List<EmployeeModel> containing the current employees.

            string json_data = JsonConvert.SerializeObject(list); //The list of employees is converted to a JSON string using  //This JSON string (json_data) will be sent back to the client.

            return Json(json_data, JsonRequestBehavior.AllowGet); //The method returns the JSON string as an ActionResult.
        }                                                          //JsonRequestBehavior.AllowGet is specified to allow HTTP GET requests for the JSON result




        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeModel empDetails)
        {
            var file = Request.Files["file"]; // Get the uploaded file
            string imagePath = empDetails.ImagePath; // Use the existing ImagePath

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName); // Get the file name
                var folderPath = Server.MapPath("~/Content/ProfileImages");

                // Check if directory exists, if not, create it
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath); // Create the directory
                }

                // Sanitize file name to remove invalid characters
                fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

                // Ensure file name is unique
                var uniqueFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid() + Path.GetExtension(fileName);
                var path = Path.Combine(folderPath, uniqueFileName);

                try
                {
                    // Save the file
                    file.SaveAs(path);
                    imagePath = "/Content/ProfileImages/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(500, $"Error saving file: {ex.Message}, StackTrace: {ex.StackTrace}");
                }
            }
            // If no new file was uploaded, the previous ImagePath is used
            empDetails.ImagePath = imagePath;

            try
            {
                // Update the employee details
                string result = _repository.UpdateEmployee(empDetails);

                // Return the updated employee list
                var employeeList = _repository.GetEmployees();
                return Json(employeeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return new HttpStatusCodeResult(500, $"Error: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }






        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            string result = _repository.DeleteEmployee(id);

            List<EmployeeModel> list = _repository.GetEmployees();

            string json_data = JsonConvert.SerializeObject(list);

            return Json(json_data, JsonRequestBehavior.AllowGet);
        }


        // Upload Excel file and insert data
        [HttpPost]
        public ActionResult AddEmployeeFromExcel(HttpPostedFileBase excelFile)
        {
            if (excelFile == null || excelFile.ContentLength == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No file uploaded.");
            }

            if (Path.GetExtension(excelFile.FileName) != ".xlsx")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid file format.");
            }

            try
            {


                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // Load the Excel file
                using (var package = new ExcelPackage(excelFile.InputStream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // Assuming row 1 is the header    a
                    {
                        string name = worksheet.Cells[row, 1].Text;
                        string email = worksheet.Cells[row, 2].Text;
                        int cityID = int.Parse(worksheet.Cells[row, 3].Text);
                        string gender = worksheet.Cells[row, 4].Text;
                        bool skills = bool.Parse(worksheet.Cells[row, 5].Text);
                        string hobbies = worksheet.Cells[row, 6].Text;
                        string CityName = worksheet.Cells[row, 7].Text;
                        // int CountryIDs = int.Parse(worksheet.Cells[row, 8].Text);
                        // string cityName = worksheet.Cells[row, 3].Text.Trim(); // Manual entry in Excel
                        //  string countryNames = worksheet.Cells[row, 7].Text.Trim(); // Manual entry in Excel

                        string CountryIDs = worksheet.Cells[row, 8].Text;
                        //  string password = worksheet.Cells[row, 8].Text;
                        //string imagePath = worksheet.Cells[row, 9].Text;

                        // Get CityID from CityName


                        // Call repository method to insert data
                        _repository.AddEmployeeFromExcel(name, email, cityID, gender, skills, hobbies, CityName, CountryIDs);
                    }
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }


    }
}