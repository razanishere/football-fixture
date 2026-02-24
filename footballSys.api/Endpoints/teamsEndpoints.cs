
using footballSys.api.Data;
using footballSys.api.Dtos;
using footballSys.api.Entities;
using footballSys.api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace footballSys.api.Endpoints;

public static class teamsEndpoints
{

    const string GetIntroEndpointName = "GetIntro";
    
    public static RouteGroupBuilder MapTeamsEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("intro");


        // ENDPOINTS, CRUD.


        // GET /intro

        
        //* go to database, then Teams table, then Select a team and transfer it to a DTO (from databse to DTO)
        group.MapGet("/", async (teamsContext dbContext) =>
            await dbContext.Teams
                    .Select(team => team.ToTeamDto())
                    .AsNoTracking()
                    .ToListAsync());

        //GET /intro/1
        group.MapGet("/{id}", async (int id, teamsContext dbContext) =>
        {
            Teams? team = await dbContext.Teams.FindAsync(id);

            return team is null ? Results.NotFound() : Results.Ok(team.ToTeamDto());
        })
        .WithName(GetIntroEndpointName);


        //POST /games
        // Dependency injection
        group.MapPost("/", async (createTeamDto newTeam, teamsContext dbContext) =>
        {
            Teams team = newTeam.ToEntity();


            dbContext.Teams.Add(team);
            await dbContext.SaveChangesAsync();

            footballTeamIntro teamIntroDto = new(
               team.Id,
               team.teamName,
               team.yearEstablished,
               team.teamLogoPath,
               team.teamColor1,
               team.teamColor2
               );

           
            return Results.CreatedAtRoute(
             GetIntroEndpointName,
             new { id = team.Id },
              team.ToTeamDto());

        }).WithParameterValidation();


        // UPDATE  - PUT

        group.MapPut("/{id}", async (int id, updateTeamDto updatedTeam, teamsContext dbContext) =>
        {
            var existingTeam = await dbContext.Teams.FindAsync(id);

            if (existingTeam is null)
            {
                return Results.NotFound();
            }

            if (!string.IsNullOrWhiteSpace(updatedTeam.teamName))
            {
                existingTeam.teamName = updatedTeam.teamName;
            }

            if (updatedTeam.yearEstablished != 0)
            {
                existingTeam.yearEstablished = updatedTeam.yearEstablished;
            }

            if (!string.IsNullOrWhiteSpace(updatedTeam.LogoURL))
            {
                existingTeam.teamLogoPath = updatedTeam.LogoURL;
            }

            if (!string.IsNullOrWhiteSpace(updatedTeam.teamColor1))
            {
                existingTeam.teamColor1 = updatedTeam.teamColor1;
            }

            if (!string.IsNullOrWhiteSpace(updatedTeam.teamColor2))
            {
                existingTeam.teamColor2 = updatedTeam.teamColor2;
            }

            // ! this one works in bulk ?
            // dbContext.Entry(existingTeam)
            //         .CurrentValues
            //         .SetValues(updatedTeam.ToEntity(id));

            await dbContext.SaveChangesAsync();

            // in updating we dont return anything
            return Results.NoContent();

        });


        // DELETE 

        group.MapDelete("/{id}", async (int id, teamsContext dbContext) =>
        {
            
            await dbContext.Teams
                        .Where(team => team.Id == id)
                        .ExecuteDeleteAsync();

            return Results.NoContent();
        });
        return group;

    }
}
