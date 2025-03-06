using System.Security.Claims;
using System.Text;
using Demos.Data;
using Demos.Models;
using Demos.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private DemosDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public AuthController(DemosDbContext context, UserManager<IdentityUser> userManager)
    {
        _dbContext = context;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public IActionResult Login([FromHeader(Name = "Authorization")] string authHeader)
    {
        try
        {
            string encodedCreds = authHeader.Substring(6).Trim();
            string creds = Encoding
                .GetEncoding("iso-8859-1")
                .GetString(Convert.FromBase64String(encodedCreds));

            // Get email and password
            int separator = creds.IndexOf(':');
            string email = creds.Substring(0, separator);
            string password = creds.Substring(separator + 1);

            var user = _dbContext.Users.Where(u => u.Email == email).FirstOrDefault();
            var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            var hasher = new PasswordHasher<IdentityUser>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (user != null && result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                foreach (var userRole in userRoles)
                {
                    var role = _dbContext.Roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                HttpContext
                    .SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    )
                    .Wait();

                return Ok();
            }

            return new UnauthorizedResult();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet]
    [Route("logout")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult Logout()
    {
        try
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("Me")]
    [Authorize]
    public IActionResult Me()
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var profile = _dbContext.UserProfiles.SingleOrDefault(up =>
            up.IdentityUserId == identityUserId
        );
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        if (profile != null)
        {
            var userDto = new UserProfileDTO
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfileImage = profile.ProfileImage,
                IsArtist = profile.IsArtist,
                JoinDate = profile.JoinDate,
                IdentityUserId = identityUserId,
                UserName = User.FindFirstValue(ClaimTypes.Name),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Roles = roles,
            };

            return Ok(userDto);
        }
        return NotFound();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationDTO registration)
    {
        try
        {
            // Create a new IdentityUser with the provided username and email
            var user = new IdentityUser
            {
                UserName = registration.UserName,
                Email = registration.Email,
            };

            // Decode the base64 password as the frontend is encoding it with btoa()
            var password = Encoding
                .GetEncoding("iso-8859-1")
                .GetString(Convert.FromBase64String(registration.Password));

            // Attempt to create the user with the provided password
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Add the user to the appropriate role based on IsArtist
                if (registration.IsArtist)
                {
                    await _userManager.AddToRoleAsync(user, "Artist");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Listener");
                }

                // Create and save the associated UserProfile with all required fields
                _dbContext.UserProfiles.Add(
                    new UserProfile
                    {
                        FirstName = registration.FirstName,
                        LastName = registration.LastName,
                        Email = registration.Email, // Make sure to include Email as it's required
                        UserName = registration.UserName, // Make sure to include UserName as it's required
                        ProfileImage = registration.ProfileImage, // Now provided by frontend
                        IsArtist = registration.IsArtist, // Now provided by frontend
                        JoinDate = DateTime.Now,
                        IdentityUserId = user.Id,
                    }
                );
                _dbContext.SaveChanges();

                // Create claims for the user's identity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                // Add role claim
                claims.Add(
                    new Claim(ClaimTypes.Role, registration.IsArtist ? "Artist" : "Listener")
                );

                // Create the claims identity for authentication
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                // Sign in the user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                return NoContent();
            }
            else
            {
                // Return specific error information to help debug registration issues
                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            return StatusCode(500, new { Error = "Registration failed", Message = ex.Message });
        }
    }
}
