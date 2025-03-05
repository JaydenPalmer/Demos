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
public class InstrumentController : ControllerBase
{
    private DemosDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public InstrumentController(DemosDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllInstruments()
    {
        try
        {
            IQueryable<Instrument> query = _dbContext.Instruments.Include(i => i.Category);

            return Ok(
                query.Select(i => new InstrumentDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category.Name,
                })
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request {ex.Message}");
        }
    }
}
