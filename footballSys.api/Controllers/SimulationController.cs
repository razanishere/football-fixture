using footballSys.api.Data;
using footballSys.api.Dtos;
using footballSys.api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//? a controller must only: trigger method, save changes, and return results

namespace footballSys.api.Controllers
{




    [ApiController]
    [Route("api/simulation")]
    public class SimulationController : ControllerBase
    {
        private readonly teamsContext _context;
        private readonly GoalSystem _goalSystem;
        private readonly ScoreGenerator _scoreGenerator;
        private readonly LeagueTableService _leagueTableService;

        public SimulationController(LeagueTableService leagueTableService, teamsContext context, GoalSystem goalSystem, ScoreGenerator scoreGenerator)
        {
            _context = context;
            _goalSystem = goalSystem;
            _scoreGenerator = scoreGenerator;
            _leagueTableService = leagueTableService;
        }

        /* Simulate matches method here
                private void SimulateMatches(List<Match> matches)
                {
                    Random rnd = new Random();

                    foreach (var match in matches)
                    {
                        if (match.homeScore != null)
                            continue;

                        _goalSystem.SimulateMatch(match, rnd);
                    }

                    _context.SaveChanges();
                }
        */

        //* controller to play all fixture
        [HttpPost("play-all/{fixtureId}")]
        public async Task<IActionResult> PlayAllMatches(int fixtureId)
        {

            var fixtureExists = _context.Matches.Any(m => m.fixtureId == fixtureId);
            if (!fixtureExists)
            {
                return NotFound("NOT FOUND!");
            }

            _scoreGenerator.GenerateScore(fixtureId);

            var matches = _context.Matches
            .Where(m => m.fixtureId == fixtureId)
            .ToList();

            var table = await _leagueTableService.CalculateTable(fixtureId);

            var champion = table.FirstOrDefault(); //TODO: link to team name

            return Ok(new
            {
                Matches = matches,
                Table = table,
                Champion = champion
            });

        }

        //* endpoint to make scores only for one week 
        [HttpPost("play-week/{fixtureId}/{week}")]
        public IActionResult PlayOneWeek(int fixtureId, int week)
        {

            var fixtureExists = _context.Matches.Any(m => m.fixtureId == fixtureId);
            if (!fixtureExists)
            {
                return NotFound("Fixture not found!");
            }

            var weekExists = _context.Matches
            .Any(m => m.fixtureId == fixtureId && m.Week == week);

            if (!weekExists)
            {
                return NotFound("Week not found!");
            }

            _scoreGenerator.GenerateScoreForWeek(fixtureId, week);

            var matches = _context.Matches
            .Where(m => m.fixtureId == fixtureId && m.Week == week)
            .Select(m => new matchResultsDto
            {
                Id = m.Id,
                HomeTeamId = m.HomeTeamId,
                AwayTeamId = m.AwayTeamId,
                HomeTeamName = _context.Teams.Where(t => t.Id == m.HomeTeamId).Select(t => t.teamName).FirstOrDefault(),
                AwayTeamName = _context.Teams.Where(t => t.Id == m.AwayTeamId).Select(t => t.teamName).FirstOrDefault(),
                HomeScore = m.homeScore,
                AwayScore = m.awayScore,
                Week = m.Week
            })
            .ToList();

            var table = _leagueTableService.CalculateTable(fixtureId, week);

            return Ok(new
            {
                Matches = matches,
                Table = table
            });
        }






        // BELOW: LEAGUE SCORE TABLE ENDPOINT 3 STATES MATCH, WEEK AND FIXTURE
        /*
                //* play one match and return updated league table
                //! LeagueOneMatch will NOT be used in the frontend. comment after finishing the project.
                [HttpPost("league-table/test/{matchId}")]
                public IActionResult LeagueOneMatch(int matchId)
                {
                    var match = _context.Matches.FirstOrDefault(m => m.Id == matchId);

                    if (match == null)
                    {
                        return NotFound("match not found");
                    }

                    if (match.homeScore != null)
                        return BadRequest("match already playd");

                    Random rnd = new Random();
                    _goalSystem.SimulateMatch(match, rnd);
                    _context.SaveChanges();

                    var table = _leagueTableService.CalculateTable(match.fixtureId);

                    return Ok(table);
                }
        */

        /*
                //*play one week (all matches in a week) and return league table

                [HttpPost("league-table/{fixtureId}/{week}")]
                public IActionResult LeagueOneWeek(int fixtureId, int week)
                {
                    var matches = _context.Matches
                        .Where(m => m.fixtureId == fixtureId && m.Week == week)
                        .ToList();

                    if (!matches.Any())
                        return NotFound();

                    SimulateMatches(matches);

                    var table = _leagueTableService.CalculateTable(fixtureId, week);

                    return Ok(new
                    {
                        Matches = matches,
                        Table = table
                    });
                }

        */

        /*
                //* return league table for all fixture AND CHAMPION of the fixture


                public IActionResult LeagueFixture(int fixtureId)
                {
                    var matches = _context.Matches
                        .Where(m => m.fixtureId == fixtureId)
                        .ToList();

                    if (!matches.Any())
                        return NotFound();

                    SimulateMatches(matches);

                    var table = _leagueTableService.CalculateTable(fixtureId);

                    return Ok(new
                    {
                        Matches = matches,
                        Table = table
                    });
                }


        */








    }
}