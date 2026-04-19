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


    [HttpPut("{id}")]
public async Task<IActionResult> UpdateTeam(
    int id,
    [FromForm] UpdateTeamFormDto updatedTeam,
    [FromForm] IFormFile? logo 
)
{
    var team = await _dbContext.Teams.FindAsync(id);
    if (team is null)
        return NotFound();

    
    if (!string.IsNullOrWhiteSpace(updatedTeam.teamName))
        team.teamName = updatedTeam.teamName;

    if (updatedTeam.yearEstablished != 0)
        team.yearEstablished = updatedTeam.yearEstablished;

    if (!string.IsNullOrWhiteSpace(updatedTeam.teamColor1))
        team.teamColor1 = updatedTeam.teamColor1;

    if (!string.IsNullOrWhiteSpace(updatedTeam.teamColor2))
        team.teamColor2 = updatedTeam.teamColor2;

    
    if (logo != null && logo.Length > 0)
    {
        
        if (!string.IsNullOrWhiteSpace(team.teamLogoPath))
        {
            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", team.teamLogoPath);
            if (System.IO.File.Exists(oldPath))
                System.IO.File.Delete(oldPath);
        }

        
        var ext = Path.GetExtension(logo.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "teamLogos", fileName);

        using var stream = new FileStream(savePath, FileMode.Create);
        await logo.CopyToAsync(stream);

        team.teamLogoPath = $"uploads/teamLogos/{fileName}";
    }

    await _dbContext.SaveChangesAsync();

    return NoContent();
}

}

