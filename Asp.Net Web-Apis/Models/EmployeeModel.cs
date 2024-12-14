using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asp.Net_Web_Apis.Models
{
    public class EmployeeModel
    {
        //all of these property
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int CityID { get; set; }
        public string CityName { get; set; }

        public string Gender { get; set; }

        public bool Skills { get; set; }
        public string Hobbies { get; set; }
        public string CountryIDs { get; set; } // Comma-separated CounttryIDs
        public string CountryName { get; set; } // Concatenated CountryNames
        public string Password { get; set; } // New Password field
        public string RePassword { get; set; } // New RePassword field
        public string ImagePath { get; set; }
        public DateTime CreatedDate { get; set; }  // Add this property

        
    }
    public class AllCities
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
    }

    public class AllCountries
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

}