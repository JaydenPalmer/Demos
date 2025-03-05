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
using Microsoft.EntityFrameworkCore;

namespace Demos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstrumentCategoryController : ControllerBase
{
    private DemosDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public InstrumentCategoryController(DemosDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllInstrumentCategories()
    {
        try
        {
            IQueryable<InstrumentCategory> query = _dbContext.InstrumentCategories.Include(ic =>
                ic.Instruments
            );

            return Ok(
                query.Select(ic => new InstrumentCategoryDTO
                {
                    Id = ic.Id,
                    Name = ic.Name,
                    Description = ic.Description,
                    InstrumentCount = ic.Instruments.Count,
                })
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request {ex.Message}");
        }
    }
}
