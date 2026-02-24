using System;
using footballSys.api.Data;
using footballSys.api.Dtos;
using footballSys.api.Entities;

namespace footballSys.api.Mapping;

public static class TeamMapping
{

    //1. Create team instance > team enitity instance

    public static Teams ToEntity(this createTeamDto team)
    {
         return  new Teams()
            {
                teamName = team.teamName,
                yearEstablished = team.yearEstablished,
                teamLogoPath = team.LogoURL,
                teamColor1 = team.teamColor1,
                teamColor2 = team.teamColor2

            };
    
   }

   public static Teams ToEntity(this updateTeamDto team, int id)
    {
        return new Teams
        {
            Id = id,
            teamName = team.teamName,
            yearEstablished = team.yearEstablished,
            teamLogoPath = team.LogoURL,
            teamColor1 = team.teamColor1,
            teamColor2 = team.teamColor2
        };
       
    }

    public static footballTeamIntro ToTeamDto(this Teams team)
    {
        return new(
                team.Id, 
                team.teamName,
                team.yearEstablished, 
                team.teamLogoPath,
                team.teamColor1,
                team.teamColor2
                );

    }

    }
