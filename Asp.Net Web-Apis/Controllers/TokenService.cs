using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Asp.Net_Web_Apis.Models;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private string key = "ThisIsASecureKeyOfAtLeast32CharactersLength!";  // Secure key

    public string GenerateToken(EmployeeModel employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.ASCII.GetBytes(this.key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, employee.Email),
                new Claim(ClaimTypes.Role, "Employee")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    //public ClaimsPrincipal ValidateToken(string token)
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.ASCII.GetBytes(this.key);
    //    try
    //    {
    //        tokenHandler.ValidateToken(token, new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateIssuer = false,
    //            ValidateAudience = false,
    //            ClockSkew = TimeSpan.Zero
    //        }, out SecurityToken validatedToken);

    //        // Return the principal (identity) extracted from the token
    //        SecurityToken validatedToken1 = validatedToken;
    //        return new ClaimsPrincipal((System.Security.Principal.IIdentity)((validatedToken1 as JwtSecurityToken)?.Claims));
    //    }
    //    catch
    //    {
    //        return null; // Return null if token is invalid
    //    }
    //}
    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this.key);

        try
        {
            // Validate the token
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Set true if you want to validate issuer
                ValidateAudience = false, // Set true if you want to validate audience
                ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
            }, out SecurityToken validatedToken);

            // Check if the token is a valid JWT
            if (validatedToken is JwtSecurityToken jwtToken)
            {
                return principal; // Return ClaimsPrincipal
            }

            return null; // If not a JWT, return null 
        }
        catch (Exception ex)
        {
            // Log the exception details for debugging
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null; // Return null if token validation fails
        }
    }

}
