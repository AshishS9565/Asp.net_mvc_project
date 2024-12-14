using Asp.Net_Web_Apis.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Controllers;

namespace Asp.Net_Web_Apis.Repository
{
    // This controller requires authentication for all endpoints unless specified otherwise  
    [RoutePrefix("api/values")] // Defines the base route for this controller
    public class ApisController : ApiController
    {
        private readonly TokenService _tokenService = new TokenService();
        private readonly Repository _repository = new Repository();

        // Override the method to check token-based authentication for each request
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            var token = controllerContext.Request.Headers.Authorization?.Parameter;

            if (token != null)
            {
                try
                {
                    var validatedToken = _tokenService.ValidateToken(token);
                    if (validatedToken == null)
                    {
                        throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
                catch
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                // Allow anonymous access to login, but ensure other endpoints require authorization
                if (!controllerContext.Request.RequestUri.ToString().Contains("/login"))
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }

        // POST: api/values/login
        [HttpPost]
        [Route("login")]
        [AllowAnonymous] // This route should be accessible without a token
        public IHttpActionResult Login([FromBody] EmployeeModel loginModel)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call repository method to verify login
            EmployeeModel employee = _repository.VerifyLogin(loginModel.Email, loginModel.Password);

            // If employee is found, return success response with a token
            if (employee != null)
            {
                string token = _tokenService.GenerateToken(employee);
                return Ok(new { message = "Login successful!", token, employee });
            }
            else
            {
                // If credentials are invalid, return unauthorized
                return Unauthorized();
            }
        }

        // GET: api/values
        [HttpGet]
        [Route("")]
      //  [Authorize] // Token required to access this endpoint
        public IEnumerable<EmployeeModel> Get()
        {
            // Call the repository to get the list of employees
            return _repository.GetEmployees();
        }

        // POST: api/values
        [HttpPost]
        [Route("")]
       // [Authorize] // Token required
        public IHttpActionResult Post([FromBody] EmployeeModel employee)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the repository to add the employee
            string result = _repository.AddEmployee(employee);

            if (!string.IsNullOrEmpty(result))
            {
                return Ok("Employee added successfully!");
            }
            else
            {
                return InternalServerError();
            }
        }

        // PUT: api/values/{id}
        [HttpPut]
        [Route("{id}")]
      //  [Authorize] // Token required
        public IHttpActionResult Put(int id, [FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string result = _repository.UpdateEmployee(id, employee);
            return Ok(result);
        }

        // DELETE: api/values/{id}
        [HttpDelete]
        [Route("{id}")]
       // [Authorize] // Token required
        public IHttpActionResult Delete(int id)
        {
            string result = _repository.DeleteEmployee(id);
            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/values/bulk-insert
        [HttpPost]
        [Route("bulk-insert")]
     //   [Authorize] // Token required
        public IHttpActionResult BulkInsert([FromBody] List<EmployeeModel> employees)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string result = _repository.BulkInsertEmployees(employees);

                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(new { message = "Employees inserted successfully!" });
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
