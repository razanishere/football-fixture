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


        //* controller to play all fixture
        [HttpPost("play-all/{fixtureId}")]
        public async Task<IActionResult> PlayAllMatches(int fixtureId)
        {

            var fixtureExists = _context.Matches.Any(m => m.fixtureId == fixtureId);
            if (!fixtureExists)
            {
                return NotFound("NOT FOUND!");
            }

            // show teams score
            var teamIdsInFixture = _context.Matches
                .Where(m => m.fixtureId == fixtureId)
                .Select(m => m.HomeTeamId)
                .Union(
                    _context.Matches
                        .Where(m => m.fixtureId == fixtureId)
                        .Select(m => m.AwayTeamId)
                )
                .Distinct();

            var teamLevels = _context.Teams
                .Where(t => teamIdsInFixture.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    t.teamName,
                    t.level
                })
                .ToList();

            _scoreGenerator.GenerateScore(fixtureId);

            var matches = _context.Matches
            .Where(m => m.fixtureId == fixtureId)
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

            var table = await _leagueTableService.CalculateTable(fixtureId);



            return Ok(new
            {
                Matches = matches,
                Table = table,
                TeamLevels = teamLevels
            });

        }

        //* endpoint to make scores only for one week 
        [HttpPost("play-week/{fixtureId}/{week}")]
        public async Task<IActionResult> PlayOneWeekAsync(int fixtureId, int week)
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

            //show team scores

            var teamIdsInFixture = _context.Matches
                .Where(m => m.fixtureId == fixtureId)
                .Select(m => m.HomeTeamId)
                .Union(
                    _context.Matches
                        .Where(m => m.fixtureId == fixtureId)
                        .Select(m => m.AwayTeamId)
                )
                .Distinct();

            var teamLevels = _context.Teams
                .Where(t => teamIdsInFixture.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    t.teamName,
                    t.level
                })
                .ToList();

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

            var table = await _leagueTableService.CalculateTable(fixtureId, week);

            return Ok(new
            {
                Matches = matches,
                Table = table,
                TeamLevels = teamLevels
            });
        }



        //* show and calculate league table
        [HttpGet("{fixtureId}/table")]
        public async Task<IActionResult> GetLeagueTable(int fixtureId, int? week = null)
        {
            var table = await _leagueTableService.CalculateTable(fixtureId, week);
            return Ok(table);
        }




        //* mark fixture as finished
        [HttpPut("{fixtureId}/finish")]
        public IActionResult FinishFixture(int fixtureId)
        {
            var fixture = _context.Fixtures.FirstOrDefault(f => f.Id == fixtureId);

            if (fixture == null)
            {
                return NotFound("Fixture not found!");
            }

            // optional safety: check all matches played
            var allPlayed = _context.Matches
                .Where(m => m.fixtureId == fixtureId)
                .All(m => m.homeScore != null && m.awayScore != null);

            if (!allPlayed)
            {
                return BadRequest("Not all matches are played yet!");
            }

            fixture.IsFinished = true;
            _context.SaveChanges();

            return Ok("Fixture marked as finished.");
        }

         //* to fetch fixture infos from Fixture table
        [HttpGet("get-fixtures")]
        public async Task<ActionResult<IEnumerable<Fixture>>> GetFixtures()
        {
            var fixtures = await _context.Fixtures.ToListAsync();
            return Ok(fixtures);
        }



    }
}