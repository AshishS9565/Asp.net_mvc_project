var app = angular.module('EmployeeApp', []);
app.controller('EmployeeController', function ($scope, $http) {
    $scope.employee = [];
    $scope.model = {};
    $scope.cities = [];
    $scope.countries = [];
    $scope.selectedCity = null;
    $scope.selectedUser = {};
    $scope.selectedCountry = null;
    $scope.message = null;
    $scope.imageFile;
    $scope.SearchText = '';
    $scope.currentPage = 1;   // Current page number
    $scope.pageSize = 5;     // Number of records per page
    $scope.totalPages = 0;    // Total number of pages

    //$scope.onFilterChange = function () {
    //   // console.log("Search Text changed:", $scope.SearchText);
    //    // Additional logic if needed when search text changes
    //};
  

    debugger;
    $http({
        method: 'GET',
        url: '/Employee/GetEmployee',
        headers: {
            'Content-type': 'application/json'
        }
    }).then(function (response) {
        debugger;
        console.log("Fetched Employee:", response.data);
        $scope.employee = JSON.parse(response.data);
        $scope.totalPages = Math.ceil($scope.employee.length / $scope.pageSize);


    }, function (error) {
        console.log(error);
    });



    // Function to get paginated employees for the current page
    $scope.getPaginatedEmployees = function () {
        var start = ($scope.currentPage - 1) * $scope.pageSize;
        var end = start + $scope.pageSize;
        return $scope.employee.slice(start, end);  // Return a subset of employees
    };

    // Function to move to the next page
    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.totalPages) {
            $scope.currentPage++;
        }
    };

    // Function to move to the previous page
    $scope.previousPage = function () {
        if ($scope.currentPage > 1) {
            $scope.currentPage--;
        }
    };




    // Function to get employees with pagination and filtering
    //debugger;
    //$scope.getEmployees = function () {

    //    // Build the request URL with query parameters for pagination and filtering
    //    var url = '/Employee/GetEmployee?pageNumber=' + $scope.currentPage + '&pageSize=' + $scope.pageSize;


    //    if ($scope.filter) {
    //        url += '&filter=' + encodeURIComponent($scope.filter);
    //    }

    //    // Fetch employee data from the server
    //    $http({
    //        method: 'GET',
    //        url: url,
    //        headers: {
    //            'Content-Type': 'application/json'
    //        }
    //    }).then(function (response) {
    //        console.log("Fetched Employees:", response.data);

    //        // Assuming the response contains totalCount and employee list
    //        $scope.totalCount = response.data.totalCount; // Total number of employees
    //        $scope.employees = response.data.employees; // Employee data
    //    }, function (error) {
    //        console.log(error);
    //    });
    //};

    //// Function to handle search/filter input changes
    //$scope.onFilterChange = function () {
    //    $scope.currentPage = 1; // Reset to the first page on filter change
    //    $scope.getEmployees(); // Fetch employees with the new filter
    //};

    //// Function to go to the previous page
    //$scope.previousPage = function () {
    //    if ($scope.currentPage > 1) {
    //        $scope.currentPage--;
    //        $scope.getEmployees(); // Fetch employees for the new page
    //    }
    //};

    // Function to go to the next page
    //$scope.nextPage = function () {
    //    if ($scope.currentPage * $scope.pageSize < $scope.totalCount) {
    //        $scope.currentPage++;
    //        $scope.getEmployees(); // Fetch employees for the new page
    //    }
    //};
    //    // Initial load of employees
    //    $scope.getEmployees();





    $http({
        method: 'GET',
        url: '/Employee/GetAllCities',
        headers: {
            'Content-type': 'application/json'
        }
    }).then(function (response) {
        debugger;
        console.log("Fetched cities:", response.data);
        $scope.cities = JSON.parse(response.data);

    }, function (error) {
        console.log(error);
    });

    $http({
        method: 'GET',
        url: '/Employee/GetAllCountries',
        headers: {
            'Content-type': 'application/json'
        }
    }).then(function (response) {

        console.log("Fetched countries:", response.data);
        $scope.countries = JSON.parse(response.data);

    }, function (error) {
        console.log(error);
    });


    //$scope.login = function () {
    //    debugger
    //    if ($scope.loginForm.$valid) {
    //        $http({
    //            method: 'POST',
    //            url: '/Employee/Login',
    //            data: $scope.model,
    //            headers: {
    //                'Content-Type': 'application/json'
    //            }
    //        }).then(function (response) {
    //            debugger;
    //            console.log("verified emp:", response.data);
    //            // On success, redirect to the List page
    //            window.location.href = '/Employee/List2';
    //        }, function (error) {
    //            // On error, display the error message
    //            alert("Invalid Email or Password");
    //           // $scope.errorMessage= 'Invalid Email or Password';
    //           // console.log(error);
    //        });
    //    } 
    //};

    $scope.login = function () {
        if ($scope.loginForm.$valid) {
            $http({
                method: 'POST',
                url: '/Employee/Login',
                data: $scope.model,
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(function (response) {
                if (response.data.success) {
                    // Login successful, redirect to List2
                    window.location.href = '/Employee/List2';
                } else {
                    // Set the error message from server response
                    $scope.errorMessage = response.data.message || 'Invalid Email or Password';
                }
            }, function (error) {
                // Handle error response, like a network error or 500 response
                $scope.errorMessage = 'An error occurred while processing your request. Please try again later.';
                console.log(error);
            });
        }
    };





    /*This is how add new scope pass all the data  data in primari table  forexample we had insert the cityid from drop down list in to employeerecord table
        we need to add cityiid column in employeerecord and cityname becouse of we want to display citname in html 
        we insert only the cityid and creat store procedure to get cityname based on the inserted cityid  for creating store procedure-->
        we need to diclear  -- Declare a variable to hold the city name
         **----->>>DECLARE @CityName NVARCHAR(50); --->>>***
             and for geting cityname-->> 
              -- Get the city name based on the city ID
        ****--->>>   SELECT @CityName = CityName FROM Cities WHERE CityID = @CityID;--->>>>*********
        also mention the CityID in the Addmodel Repository 

        */
    // Function to collect selected city IDs
    //$scope.getSelectedCountryID = function () {
    //    var CityID = [];
    //    angular.forEach($scope.selectedCities, function (value, key) {
    //        if (value) {
    //            CityID.push(value);
    //        }
    //    });
    //    return CityID;
    //};



    $scope.AddEmployee = function () {
        debugger;
        //var selectedCityID = $scope.getSelectedCityID();
        if ($scope.model.Password !== $scope.model.RePassword) {
            alert("Passwords do not match!");
            return;
        }

        var hobbies = [];
        angular.forEach($scope.model.Hobbies, function (value, key) {
            if (value) {
                hobbies.push(value);
            }
        });

        // Filter method returns a new array containing the elements that pass a certain test performed on an original array.
        // Map method is used to apply a function on every element in an array and returns a new array of the same size as the input array.

        var selectedCountries = $scope.countries.filter(function (country) {
            return country.selected;
        }).map(function (country) {
            return country.CountryID;
        });

        //For Handel upload imagee

        var formData = new FormData();
        formData.append('Name', $scope.model.Name);
        formData.append('Email', $scope.model.Email);
        formData.append('Password', $scope.model.Password);
        formData.append('CityID', $scope.selectedCity.CityID);
        formData.append('CountryIDs', selectedCountries.join(','));
        formData.append('Gender', $scope.model.Gender);
        formData.append('Skills', $scope.model.Skills);
        formData.append('Hobbies', hobbies.join(','));

        // Append the image file if it exists
        var imageFile = document.getElementById('imageFile').files[0];
        if (imageFile) {
            formData.append('file', imageFile);
        }

        $http.post('/Employee/AddEmployee', formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            debugger
            console.log("UUUUpdatImagepat", imageFile);
            console.log("Employee added successfully:", response.data);

            // Update the employee list with the new employee data
            $scope.employee = JSON.parse(response.data);

            $scope.message = "Employee added successfully";
            $("#AddModal").modal("hide");

            // Optionally clear the form fields
            $scope.model = {};
            document.getElementById('imageFile').value = null; // Clear the file input


            $scope.message = "Employee added Successfully";
            $("#AddModal").modal("hide");
        }, function (error) {
            console.log(error);
        });

    };



    //$scope.AddEmployee = function () {
    //    debugger;
    //    //var selectedCityID = $scope.getSelectedCityID();
    //    if ($scope.model.Password !== $scope.model.RePassword) {
    //        alert("Passwords do not match!");
    //        return;
    //    }

    //    var hobbies = [];
    //    angular.forEach($scope.model.Hobbies, function (value, key) {
    //        if (value) {
    //            hobbies.push(value);
    //        }
    //    });

    //     //Filter method returns a new array containing the elements that pass a certain test performed on an original array.
    //     //Map method is used to apply a function on every element in an array and returns a new array of the same size as the input array.

    //    var selectedCountries = $scope.countries.filter(function (country) {
    //        return country.selected;
    //    }).map(function (country) {
    //        return country.CountryID;
    //    });


    //     var newEmployee = {
    //        Name: $scope.model.Name,
    //         Email: $scope.model.Email,
    //         Password: $scope.model.Password,
    //         CityID: $scope.selectedCity.CityID,
    //         CountryIDs: selectedCountries.join(',') ,// Comma-separated Country IDs
    //         Gender: $scope.model.Gender,
    //         Skills: $scope.model.Skills,
    //         Hobbies: hobbies.join(' , ') // Join the hobbies array into a comma-separated string

    //       };

    //    $http({
    //        method: 'POST',
    //        url: '/Employee/AddEmployee',
    //        data: newEmployee,
    //        headers: {
    //            'Content-type': 'application/json'
    //        }
    //    }).then(function (response) {
    //        console.log("insert Employee data:", response.data);
    //        $scope.employee = JSON.parse(response.data);
    //        $scope.message = "Employee added Successfully";

    //        $("#AddModal").modal("hide");
    //    }, function (error) {
    //        console.log(error);
    //    });
    //};

    $scope.AddEmployeeFromExcel = function () {
        //  var file = $scope.excelFile;
        debugger
        // Access the file input element by its ID
        var fileInput = document.getElementById('excelfile');
        var file = fileInput.files[0];  // Get the first file from the input

        // Check if file is selected
        if (!file) {
            alert('Please select an Excel file!');
            return;
        }

        var formData = new FormData();
        formData.append('excelFile', file);

        // Send file to the server
        $http.post('/Employee/AddEmployeeFromExcel', formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
            .then(function (response) {
                alert('File uploaded successfully!');
                console.log(response.data);
              
            })
            .catch(function (error) {
                alert('Error uploading file.');
                console.error(error);
            });
        // Update the employee list with the new employee data
        $scope.employee = JSON.parse(response.data);
    };







    /*

    $scope.selectUser = function (names) {
        $scope.selectedUser = names;
    }
    */
    $scope.selectUser = function (emp) {

        $scope.selectedUser = angular.copy(emp);
        console.log("Selected User:", $scope.selectedUser);
        $scope.selectedCity = $scope.cities.find(city => city.CityID === emp.CityID);

        // Handle hobbies: split the Hobbies string into an array and populate model.Hobbies
        var selectedHobbies = emp.Hobbies ? emp.Hobbies.split(',').map(hobby => hobby.trim()) : [];
        // Initialize model.Hobbies with the selected hobbies
        $scope.model.Hobbies = {
            Cricket: selectedHobbies.includes('Cricket') ? 'Cricket' : '',
            Music: selectedHobbies.includes('Music') ? 'Music' : '',
            Khokho: selectedHobbies.includes('Khokho') ? 'Khokho' : ''
        };
        // Ensure existing image path is set
        $scope.selectedUser.ImagePath = emp.ImagePath || ""; // Set the image path if it exists;

        // Update selected countries based on emp.CountryIDs (comma-separated string of IDs)
        var selectedCountryIDs = emp.CountryIDs ? emp.CountryIDs.split(',').map(id => id.trim()) : [];
        console.log("Selected CountryIDs:", selectedCountryIDs);

        // Loop through each country and set 'selected' based on the CountryID
        $scope.countries.forEach(function (country) {
            country.selected = selectedCountryIDs.includes(country.CountryID.toString());
        });

        console.log("Updated Countries Data:", $scope.countries);

    };


    // Function to handle file selection
    $scope.selectFile = function (element) {
        $scope.$apply(function (scope) {
            var file = element.files[0]; // Get the selected file
            $scope.employee.imageFile = file; // Store it in the employee object
        });
    };

    $scope.UpdateEmployee = function () {
        debugger
        var hobbies = [];
        angular.forEach($scope.model.Hobbies, function (value, key) {
            if (value) {
                hobbies.push(key);  // Using 'key' instead of 'value' for correct hobby names
            }
        });

        var selectedCountries = $scope.countries.filter(function (country) {
            return country.selected;
        }).map(function (country) {
            return country.CountryID;
        });

        // Create FormData object
        var formData = new FormData();
        // Only append the file if the user has selected a new one
        if ($scope.employee.imageFile) {
            formData.append("file", $scope.employee.imageFile); // The image file
        }

        //formData.append('file', $scope.employee.imageFile); // Append selected image file if any
        formData.append('ID', $scope.selectedUser.ID);
        formData.append('Name', $scope.selectedUser.Name);
        formData.append('Email', $scope.selectedUser.Email);
        formData.append('CityID', $scope.selectedCity.CityID);
        formData.append('Gender', $scope.selectedUser.Gender);
        formData.append('Skills', $scope.selectedUser.Skills);
        formData.append('CountryIDs', selectedCountries.join(','));
        formData.append('Hobbies', hobbies.join(','));
        // Append the existing ImagePath so it is preserved if no new image is uploaded
        formData.append("ImagePath", $scope.selectedUser.ImagePath);

        // HTTP POST request to the server
        $http.post('/Employee/UpdateEmployee', formData, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        }).then(function (response) {
            debugger

            console.log("UUUUpdatImagepat", imageFile);
            console.log("Imagepath", $scope.selectedUser.ImagePath);
            console.log("Employee updated successfully:", response.data);
            // Update the scope's employee list with the new data
            $scope.employee = (response.data); // Update the full employee list
            //   $scope.employees = response.data.data;  // Update the full employee list
            $scope.message = "Employee updated successfully";
            $("#EditModal").modal("hide");
            $scope.selectedUser = {};  // Reset selected user object if needed

        }, function (error) {
            console.log("Error updating employee:", error);
        });
    };












    //$scope.UpdateEmployee = function () {

    //    console.log("Selected City:", $scope.selectedCity); // Add this line

    //    var hobbies = [];
    //    angular.forEach($scope.model.Hobbies, function (value, key) {
    //        if (value) {
    //            hobbies.push(value);
    //        }
    //    });

    //    var selectedCountries = $scope.countries.filter(function (country) {
    //        return country.selected;
    //    }).map(function (country) {
    //        return country.CountryID;
    //    });

    //    var updatedEmployee = {
    //        ID: $scope.selectedUser.ID,
    //        Name: $scope.selectedUser.Name,
    //        Email: $scope.selectedUser.Email,
    //        CityID: $scope.selectedCity.CityID,
    //        Gender: $scope.selectedUser.Gender,
    //        Skills: $scope.selectedUser.Skills,
    //        CountryIDs: selectedCountries.join(',') ,// Comma-separated Country IDs
    //        Hobbies: hobbies.join(','), // Join the hobbies array into a comma-separated string
    //        //CountryIDs: $scope.selectedUser.CountryIDs ||''
    //        ImagePath:$scope.selectedUser.ImagePath
    //    };

    //    console.log("Updated Employee:", updatedEmployee);
    //    $http({
    //        method: 'POST',
    //        url: '/Employee/UpdateEmployee',
    //        data:updatedEmployee,
    //        headers: {
    //            'Content-type': 'application/json'
    //        }
    //    }).then(function (response) {
    //        debugger;
    //        console.log("Update kiya hua :", response.data);
    //        $scope.employee = JSON.parse(response.data);
    //        $scope.message = "Employee updated Successfully";
    //        $("#EditModal").modal("hide");
    //    }, function (error) {
    //        console.log(error);
    //    });
    //}



    $scope.DeleteEmployee = function (names) {

        $http({
            method: 'POST',
            url: '/Employee/DeleteEmployee',
            data: names,
            headers: {
                'Content-type': 'application/json'
            }
        }).then(function (response) {
            $scope.employee = JSON.parse(response.data);
            $scope.message = "Employee Deleted Successfully";
            $("#EditModal").modal("hide");
        }, function (error) {
            console.log(error);
        });
    }




    $scope.clearModel = function () {
        $scope.model = null;
        $scope.selectedCity = null;


    }

});



//$scope.uploadImage = function () {
//    if ($scope.uploadForm.$valid && $scope.imageFile) {
//        Upload.upload({
//            url: '/Employee/UploadImage',
//            data: { file: $scope.imageFile }
//        }).then(function (response) {
//            $scope.uploadMessage = "Image uploaded successfully!";
//            $scope.imagePath = response.data.filePath; // Save the file path to show the uploaded image
//        }, function (error) {
//            $scope.uploadMessage = "Error uploading image!";
//            console.log(error);
//        });
//    }
//};


/* THIS IS Store Procedure  HOW TO IMPLEMENT CITYNAME BASE ON THE wich CITYID INSERTED


CREATE PROCEDURE InsertEmployee
    @Name NVARCHAR(50),
    @Email NVARCHAR(50),
    @CityID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare a variable to hold the city name
    DECLARE @CityName NVARCHAR(50);

    -- Get the city name based on the city ID
    SELECT @CityName = CityName FROM Cities WHERE CityID = @CityID;

    -- Insert the employee with the city ID and city name
    INSERT INTO Employee (Name, Email, CityID, CityName)
    VALUES (@Name, @Email, @CityID, @CityName);
END
 */


/*

var app = angular.module('EmployeeApp', []);
    app.controller('EmployeeController', function ($scope, $http) {

        // Initialize model
        $scope.model = {};

    // Fetch employee data

        $http({
            method: 'GET',
            url: '/Employee/GetEmployee',
            headers: {
                'Content-type': 'application/json'
            }
        }).then(function (response) {
            debugger;
            $scope.employee = JSON.parse(response.data);
        }, function (error) {
            console.log(error);
        });



   /* $http({
        method: 'GET',
    url: '/Employee/GetEmployee',
    headers: {
        'Content-Type': 'application/json'
            }
        }).then(function (response) {
        $scope.employee = response.data; // No need to JSON.parse here, as response.data is already parsed
        }, function (error) {
        console.log(error);
        });*/

// Add employee/

/*
    $scope.AddEmployee = function () {
        $http({
            method: 'POST',
            url: '/Employee/AddEmployee',
            data: $scope.model,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            $scope.employee = JSON.parse(response.data); // Update employee list with response data
            $scope.message = "Employee added successfully";
            $("#AddModal").modal("hide"); // Hide the Add modal
        }, function (error) {
            console.log(error);
        });
        };

    // Select employee for editing
    $scope.selectUser = function (employee) {
        $scope.selectedUser = angular.copy(employee); // Use angular.copy to avoid two-way binding issues
        };

    // Update employee
    $scope.UpdateEmployee = function () {
        $http({
            method: 'POST',
            url: '/Employee/UpdateEmployee',
            data: $scope.selectedUser,
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            $scope.employee = JSON.parse(response.data); // Update employee list with response data
            $scope.message = "Employee updated successfully";
            $("#EditModal").modal("hide"); // Hide the Edit modal
        }, function (error) {
            console.log(error);
        });
        };


    // Delete employee
    $scope.DeleteEmployee = function (id) {
        $http({
            method: 'POST',
            url: '/Employee/DeleteEmployee',
            data: { ID: id }, // Send ID in data object
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(function (response) {
            $scope.employee = JSON.parse(response.data); // Update employee list with response data
            $scope.message = "Employee deleted successfully";
            $("#EditModal").modal("hide"); // Hide the Edit modal
        }, function (error) {
            console.log(error);
        });
        };

    // Clear model data
    $scope.clearModel = function () {
        $scope.model = {};
    $scope.selectedUser = { };
        };

    });

        */


/*

  ALTER PROCEDURE [dbo].[InsertEmployee] 
    @Name nvarchar(50),
    @Email nvarchar(50),
    @CityIDs NVARCHAR(MAX), -- Comma-separated city IDs
    @Gender nvarchar(20),
    @Skills bit,
    @Hobbies nvarchar(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Declare variables
    DECLARE @CityNames NVARCHAR(MAX);
    DECLARE @EmployeeID INT;
    
    -- Retrieve city names from the CityIDs
    DECLARE @XML XML;
    SET @XML = CAST('<root><r>' + REPLACE(@CityIDs, ',', '</r><r>') + '</r></root>' AS XML);
    
    SELECT @CityNames = STUFF((
        SELECT ', ' + CityName
        FROM Cities
        WHERE CityID IN (SELECT r.value('.', 'INT') FROM @XML.nodes('//root/r') AS RECORDS(r))
        FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)'), 1, 2, '', '');

    -- Insert the employee with the city names
    INSERT INTO EmployeeRecord (Name, Email, CityID, CityName, Gender, Skills, Hobbies)
    SELECT 
        @Name, 
        @Email,
        CityID,
        CityName,
        @Gender,
        @Skills,
        @Hobbies
    FROM Cities
    WHERE CityID IN (SELECT r.value('.', 'INT') FROM @XML.nodes('//root/r') AS RECORDS(r));
    
    -- Optionally, insert a record in a junction table if it exists
    -- Insert into EmployeeCity table for city associations if needed
    -- Example code for inserting into EmployeeCity table
    -- INSERT INTO EmployeeCity (EmployeeID, CityID)
    -- SELECT @EmployeeID, r.value('.', 'INT')
    -- FROM @XML.nodes('//root/r') AS RECORDS(r);
END



*/



/*FILTER AND MAP FUNCTION EXAMPLE-->>

/* Printing the name of students who play
basketball using filter and map method
simultaneously. 

// Taking an array of Student object
let students = [
    { id: "001", name: "Anish", sports: "Cricket" },
    { id: "002", name: "Smriti", sports: "Basketball" },
    { id: "003", name: "Rahul", sports: "Cricket" },
    { id: "004", name: "Bakul", sports: "Basketball" },
    { id: "005", name: "Nikita", sports: "Hockey" }
]

/* Applying filter function on students array to
retrieve those students Objects who play
basketball and then the new array returned by
filter method is mapped in order to get the name
of basketball players instead of whole object.
let basketballPlayers = students.filter(function (student) {
    return student.sports === "Basketball";
}).map(function (student) {
    return student.name;
})

console.log("Basketball Players are:");

 Printing out the name of Basketball players
basketballPlayers.forEach(function (players) {
    console.log(players);
});
        
        
        
        
        */