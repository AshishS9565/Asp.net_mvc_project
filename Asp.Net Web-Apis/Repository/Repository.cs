using Asp.Net_Web_Apis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace Asp.Net_Web_Apis.Repository
{
    public class Repository:IRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;//To access connection string from any class you can use the code below.


        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> list = new List<EmployeeModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.SelectEmployee", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            list.Add(new EmployeeModel
                            {
                                Name = row["Name"].ToString().Trim(),
                                Email = row["Email"].ToString().Trim(),
                                CityName = row["CityName"].ToString().Trim(),
                                Gender = row["Gender"].ToString().Trim(),
                                Skills = bool.Parse(row["Skills"].ToString()),
                                Hobbies = row["Hobbies"].ToString().Trim(),
                                CountryName = row["CountryName"].ToString().Trim(),
                                CityID = int.Parse(row["CityID"].ToString()),
                                ID = int.Parse(row["ID"].ToString()),
                                CountryIDs = row["CountryIDs"].ToString().Trim(),
                                // Check for DBNull and handle accordingly
                                ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString().Trim(),
                                // Convert CreatedDate from the DataRow to DateTime
                                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),


                            });
                        }
                    }
                }

            }
            return list;
        }
        //public List<EmployeeModel> GetEmployees(string filter = null, int pageNumber = 1, int pageSize = 3)
        //{
        //    List<EmployeeModel> list = new List<EmployeeModel>();

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand("dbo.SelectEmployee", conn);
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);

        //        // Check if there are any results
        //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            // Loop through the result set and populate the employee list
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                list.Add(new EmployeeModel
        //                {
        //                    Name = row["Name"].ToString().Trim(),
        //                    Email = row["Email"].ToString().Trim(),
        //                    CityName = row["CityName"].ToString().Trim(),
        //                    Gender = row["Gender"].ToString().Trim(),
        //                    Skills = bool.Parse(row["Skills"].ToString()),
        //                    Hobbies = row["Hobbies"].ToString().Trim(),
        //                    CountryName = row["CountryName"].ToString().Trim(),
        //                    CityID = int.Parse(row["CityID"].ToString()),
        //                    ID = int.Parse(row["ID"].ToString()),
        //                    CountryIDs = row["CountryIDs"].ToString().Trim(),
        //                    ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString().Trim(),
        //                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
        //                });
        //            }
        //        }
        //    }

        //    // Apply filtering 
        //    if (!string.IsNullOrWhiteSpace(filter))
        //    {
        //        list = list
        //            .Where(emp => emp.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
        //                          emp.Email.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
        //            .ToList();
        //    }

        //    // Apply pagination
        //    list = list
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    return list;
        //}


        //add 
        public string AddEmployee(EmployeeModel empDetails)
        {
            string response = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertEmployee", conn);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 20).Value = empDetails.Name;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 20).Value = empDetails.Email;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 256).Value = empDetails.Password;
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = empDetails.CityID;   //which i want to inseert from drop down list it must add in the this
                cmd.Parameters.Add("@CountryIDs", SqlDbType.NVarChar, -1).Value = empDetails.CountryIDs;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = empDetails.Gender;
                cmd.Parameters.Add("@Skills", SqlDbType.Bit).Value = empDetails.Skills;
                cmd.Parameters.Add("@Hobbies", SqlDbType.NVarChar, -1).Value = empDetails.Hobbies;
                cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255).Value = empDetails.ImagePath; // Handle image path
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.ExecuteNonQuery();
                response = "Success";
                conn.Close();
            }
            return response;
        }


        // store procedure for for update the employee record 
        public string UpdateEmployee( int id ,EmployeeModel empDetails)
        {
            string response = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmployee", conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = empDetails.ID;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 20).Value = empDetails.Name;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 20).Value = empDetails.Email;
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = empDetails.CityID;
                cmd.Parameters.Add("@CountryIDs", SqlDbType.NVarChar, -1).Value = empDetails.CountryIDs;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = empDetails.Gender;
                cmd.Parameters.Add("@Skills", SqlDbType.Bit).Value = empDetails.Skills;
                cmd.Parameters.Add("@Hobbies", SqlDbType.NVarChar, -1).Value = empDetails.Hobbies;
                cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255).Value = empDetails.ImagePath; // Handle image path
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                cmd.ExecuteNonQuery();
                response = "Success";
                conn.Close();
            }
            return response;
        }

        //for delete 
        public string DeleteEmployee(int id)
        {
            string response = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteEmployee", conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                cmd.ExecuteNonQuery();
                response = "Success";
                conn.Close();
            }
            return response;
        }




        public EmployeeModel VerifyLogin(string Email, string Password)
        {
            EmployeeModel emp = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.VerifyLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = Email;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 256).Value = Password;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    emp = new EmployeeModel
                    {
                        ID = int.Parse(reader["ID"].ToString()),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        CityID = int.Parse(reader["CityID"].ToString()),
                        CityName = reader["CityName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Skills = bool.Parse(reader["Skills"].ToString()),
                        Hobbies = reader["Hobbies"].ToString(),
                        CountryIDs = reader["CountryIDs"].ToString(),
                        CountryName = reader["CountryName"].ToString(),
                        ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString().Trim()

                    };
                }
                conn.Close();
            }
            return emp;
        }
        //BulkInsertEmployee
        public string BulkInsertEmployees(List<EmployeeModel> employees)
        {
            string response = "";
            var table = new DataTable();

            // Define the columns in the DataTable
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("CityID", typeof(int));
            table.Columns.Add("CityName", typeof(string));
            table.Columns.Add("Gender", typeof(string));
            table.Columns.Add("Skills", typeof(bool));  // Assuming Skills is stored as a string or could be a bit/boolean
            table.Columns.Add("Hobbies", typeof(string));
            table.Columns.Add("CountryName", typeof(string));
            table.Columns.Add("CountryIDs", typeof(string));  // Assuming CountryIDs are stored as a comma-separated string
            table.Columns.Add("Password", typeof(string));
            table.Columns.Add("ImagePath", typeof(string));

            // Loop through each employee and add rows to the DataTable
            foreach (var employee in employees)
            {
                table.Rows.Add(
                    employee.Name,
                    employee.Email,
                    employee.CityID,
                    employee.CityName,
                    employee.Gender,
                    employee.Skills,
                    employee.Hobbies,
                    employee.CountryName,
                    employee.CountryIDs,
                    employee.Password,
                    employee.ImagePath
                );
            }

            // Perform the bulk insert using a stored procedure
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("BulkInsertEmployees", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Pass the DataTable as a structured parameter
                    var parameter = command.Parameters.AddWithValue("@EmployeeTableType", table);
                    parameter.SqlDbType = SqlDbType.Structured;

                    // Execute the command
                    command.ExecuteNonQuery();

                    // Set response to indicate success
                    response = "Success";
                }
            }

            // Return the response
            return response;
        }

        //void IRepository.BulkInsertEmployees(List<EmployeeModel> employees)
        //{
        //    throw new NotImplementedException();
        //}

        //for fetch Cities table

        public List<AllCities> GetAllCities()
        {
            List<AllCities> list = new List<AllCities>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetAllCities", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            list.Add(new AllCities
                            {
                                CityName = row["CityName"].ToString().Trim(),
                                CityID = int.Parse(row["CityID"].ToString())


                            });
                        }
                    }
                }

            }
            return list;
        }




        //fetch all contries from contries table 
        public List<AllCountries> GetAllCountries()
        {
            List<AllCountries> list = new List<AllCountries>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetAllCountries", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            list.Add(new AllCountries
                            {
                                CountryName = row["CountryName"].ToString().Trim(),
                                CountryID = int.Parse(row["CountryID"].ToString())


                            });
                        }
                    }
                }

            }
            return list;
        }


        //Update employee 

        public string UpdateEmployee(EmployeeModel empDetails)
        {
            string response = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateEmployee", conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = empDetails.ID;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 20).Value = empDetails.Name;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 20).Value = empDetails.Email;
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = empDetails.CityID;
                cmd.Parameters.Add("@CountryIDs", SqlDbType.NVarChar, -1).Value = empDetails.CountryIDs;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 20).Value = empDetails.Gender;
                cmd.Parameters.Add("@Skills", SqlDbType.Bit).Value = empDetails.Skills;
                cmd.Parameters.Add("@Hobbies", SqlDbType.NVarChar, -1).Value = empDetails.Hobbies;
                cmd.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 255).Value = empDetails.ImagePath; // Handle image path
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                cmd.ExecuteNonQuery();
                response = "Success";
                conn.Close();
            }
            return response;
        }
        //add from excel
        public string AddEmployeeFromExcel(string Name, string Email, int CityID, string Gender, bool Skills, string Hobbies, string CityName, string CountryIDs)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertEmployeeFromExcel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@CityID", CityID);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@Skills", Skills);
                    cmd.Parameters.AddWithValue("@Hobbies", Hobbies);
                    cmd.Parameters.AddWithValue("@CityName", CityName);
                    cmd.Parameters.AddWithValue("@CountryIDs", CountryIDs);
                    //  cmd.Parameters.AddWithValue("@Password", Password);
                    //cmd.Parameters.AddWithValue("@ImagePath", ImagePath);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return "Success";
        }



    }
}
