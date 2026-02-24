using footballSys.api.Data;
using footballSys.api.Dtos;
using footballSys.api.Entities;
using footballSys.api.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace FootballSys.Api.Controllers;

[ApiController]
[Route("api/teams")]
public class TeamsController : ControllerBase
{
    private readonly teamsContext _dbContext;

    public TeamsController(teamsContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    [HttpPost]
    public async Task<IActionResult> TestPost(
    [FromForm] CreateTeamFormDto newTeam,
    [FromForm] IFormFile logo
)
    {
        if (logo == null || logo.Length == 0)
            return BadRequest("Logo file is required");

        // generate unique file name
        var fileExtension = Path.GetExtension(logo.FileName);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        // physical path
        var savePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "teamLogos",
            fileName
        );

        
        // save file
        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await logo.CopyToAsync(stream);
        }

        // relative path ( goes to DB)
        var logoPath = $"uploads/teamLogos/{fileName}";

        // map to entity
        Teams team = new Teams
        {
            teamName = newTeam.teamName,
            yearEstablished = newTeam.yearEstablished,
            teamColor1 = newTeam.teamColor1,
            teamColor2 = newTeam.teamColor2,
            teamLogoPath = logoPath
        };

        _dbContext.Teams.Add(team);
        await _dbContext.SaveChangesAsync();

        return Ok(team);
    }


}

