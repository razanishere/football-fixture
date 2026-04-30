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
        public IActionResult GenerateFixtures([FromBody] GenerateFixtureRequest request)
        {
            var selectedTeams = _context.Teams
                .Where(t => request.TeamIds.Contains(t.Id))
                .ToList();

            var teamIdList = selectedTeams
                .Select(t => t.Id)
                .ToList();


            _fixtureGenerator.InitializeFixtureSettings(teamIdList);


            var result = _fixtureGenerator.CreateFixtures(request.FixtureName);
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


        //* send fixture using fixtureID
        [HttpGet("{fixtureId}")]
        public IActionResult GetFixtureById(int fixtureId)
        {
            var fixtureExists = _context.Fixtures.Any(f => f.Id == fixtureId);

            if (!fixtureExists)
            {
                return NotFound("Fixture not found!");
            }

            var matches = _context.Matches
                .Where(m => m.fixtureId == fixtureId)
                .ToList();

            if (!matches.Any())
            {
                return NotFound("No matches found for this fixture!");
            }

            var teamIds = matches
                .Select(m => m.HomeTeamId)
                .Union(matches.Select(m => m.AwayTeamId))
                .Distinct();

            var teams = _context.Teams
                .Where(t => teamIds.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    t.teamName,
                    t.level
                })
                .ToList();

            var fixtures = matches
                .GroupBy(m => m.Week)
                .OrderBy(g => g.Key)
                .Select(g => g.Select(m => new
                {
                    m.Id,
                    m.HomeTeamId,
                    m.AwayTeamId,
                    HomeTeamName = _context.Teams.First(t => t.Id == m.HomeTeamId).teamName,
                    AwayTeamName = _context.Teams.First(t => t.Id == m.AwayTeamId).teamName,
                    m.Week
                }).ToList())
                .ToList();

            var playedWeeks = matches
                .Where(m => m.isPlayed == 1)
                .Select(m => m.Week)
                .Distinct()
                .OrderBy(w => w)
                .ToList();

            return Ok(new
            {
                FixtureId = fixtureId,
                Fixtures = fixtures,
                Teams = teams,
                PlayedWeeks = playedWeeks
            });
        }

        // BELOW: MATCH RESULTS ENDPOINTS

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




    }





}

