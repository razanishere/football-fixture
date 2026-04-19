using Microsoft.AspNetCore.Mvc;
using footballSys.api.Services;
using footballSys.api.Entities;
using footballSys.api.Data;
using footballSys.api.DTOs;
using System.Xml;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace footballSys.api.Controllers
{
    [ApiController]
    [Route("api/fixtures")]
    public class FixturesController : ControllerBase
    {
        private readonly teamsContext _context;
        private readonly FixtureGenerator _fixtureGenerator;


        public FixturesController(teamsContext context, FixtureGenerator fixtureGenerator)
        {
            _context = context;
            _fixtureGenerator = fixtureGenerator;
        }

        [HttpPost("generate")]
        public IActionResult GenerateFixtures([FromBody] List<int> teamIds)
        {
            var selectedTeams = _context.Teams
                .Where(t => teamIds.Contains(t.Id))
                .ToList();

            var teamIdList = selectedTeams
                .Select(t => t.Id)
                .ToList();


            _fixtureGenerator.InitializeFixtureSettings(teamIdList);


            var result = _fixtureGenerator.CreateFixtures();
            var fixtures = result.fixtures;
            var fixtureId = result.fixtureId;

            var teamDictionary = selectedTeams.ToDictionary(t => t.Id, t => t.teamName);

            var fixturesWithNames = new List<List<MatchWithNamesDTO>>();

            //* mapping the dictionary of names with the ids from the algorithm
            for (int weekIndex = 0; weekIndex < fixtures.Count; weekIndex++)
            {
                var weekMatches = fixtures[weekIndex];
                var dtoWeek = new List<MatchWithNamesDTO>();

                foreach (var match in weekMatches)
                {
                    var dto = new MatchWithNamesDTO
                    {
                        HomeTeamId = match.HomeTeamId,
                        AwayTeamId = match.AwayTeamId,
                        HomeTeamName = teamDictionary[match.HomeTeamId],
                        AwayTeamName = teamDictionary[match.AwayTeamId],
                        week = weekIndex + 1

                    };
                    dtoWeek.Add(dto);
                }

                fixturesWithNames.Add(dtoWeek);
            }

            return Ok(new
            {
                FixtureId = fixtureId,
                Fixtures = fixturesWithNames
            });

        }


        //! BELOW: MATCH RESULTS ENDPOINTS

        //* show all matches with results
        [HttpGet("{fixtureId}/matches")]
        public IActionResult GetMatchResults(int fixtureId)
        {
            var fixtureExists = _context.Matches.Any(m => m.fixtureId == fixtureId);
            if (!fixtureExists)
            {
                return NotFound("fixture not found!");
            }

            var matches = _context.Matches
            .Where(x => x.fixtureId == fixtureId)
            .Select(x => new
            {
                x.Id,
                x.Week,
                HomeTeamName = _context.Teams.FirstOrDefault(t => t.Id == x.HomeTeamId).teamName,
                AwayTeamName = _context.Teams.FirstOrDefault(t => t.Id == x.AwayTeamId).teamName,
                x.homeScore,
                x.awayScore
            })
            .ToList();

            return Ok(matches);

        }

        //* the match results of this week's matches
        [HttpGet("{fixtureId}/weeks/{week}")]
        public IActionResult GetWeekMatchResults(int fixtureId, int week)
        {
            var matches = _context.Matches
            .Where(m => m.fixtureId == fixtureId && m.Week == week)
            .Select(x => new
            {
                x.Id,
                x.Week,
                HomeTeamName = _context.Teams.FirstOrDefault(t => t.Id == x.HomeTeamId).teamName,
                AwayTeamName = _context.Teams.FirstOrDefault(t => t.Id == x.AwayTeamId).teamName,
                x.homeScore,
                x.awayScore,
                x.isPlayed
            })
            .ToList();

            return Ok(matches);

        }

        /*
                //* method to show only ONE matches results 
                [HttpGet("{matchId}/match-results")]
                public IActionResult GetSingleMatchResults(int matchId)
                {
                    var match = _context.Matches
                    .Where(m => m.Id == matchId)
                    .Select(x => new
                    {
                        x.Id,
                        x.Week,
                        x.HomeTeamId, //TODO: LINK TO TEAMS AND VIEW NAMES
                        x.AwayTeamId, //TODO: LINK TO TEAMS AND VIEW NAMES
                        x.homeScore,
                        x.awayScore
                    })
                    .ToList();

                    return Ok(match);
                }

                */







    }


}

