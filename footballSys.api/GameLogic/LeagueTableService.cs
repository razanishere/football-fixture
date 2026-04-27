using System.Threading.Tasks;
using footballSys.api.Data;
using footballSys.api.Entities;
using Microsoft.EntityFrameworkCore;

//* the league table gets calculated based on played matches.
//* when viewed on week 3 out of 6, it will show the results up to week 3.

public class LeagueTableService
{
    private readonly teamsContext _context;

    public LeagueTableService(teamsContext context)
    {
        _context = context;
    }

    //! understand this code
    //* method to calculate league table dynamically on ASKED WEEK 
    public async Task<List<leagueTableDTO>> CalculateTable(int fixtureId, int? upToWeek = null)
    {
        var matchesList = _context.Matches
        .Where(m => m.fixtureId == fixtureId &&
        m.homeScore != null &&
        m.awayScore != null);

        // filter by week (if provided)
        if (upToWeek.HasValue)
        {
            // give me all matches where theyre equal or less than the given week number
            matchesList = matchesList.Where(m => m.Week <= upToWeek.Value);
        }

        var matches = await matchesList.ToListAsync();

        var teams = await _context.Teams
    .Where(t => matches.Select(m => m.HomeTeamId)
        .Union(matches.Select(m => m.AwayTeamId))
        .Contains(t.Id))
    .ToDictionaryAsync(t => t.Id, t => t.teamName);

        var table = new Dictionary<int, leagueTableDTO>();

        foreach (var match in matches)
        {
            int homeId = match.HomeTeamId;
            int awayId = match.AwayTeamId;

            if (!table.ContainsKey(homeId))
            {
                table[homeId] = new leagueTableDTO
                {
                    TeamId = homeId,
                    TeamName = teams[homeId],
                    Played = 0,
                    Wins = 0,
                    Draws = 0,
                    Losses = 0,
                    GoalsScored = 0,
                    GoalsConceded = 0,
                    Points = 0
                };
            }

            if (!table.ContainsKey(awayId))
            {
                table[awayId] = new leagueTableDTO
                {
                    TeamId = awayId,
                    TeamName = teams[awayId],
                    Played = 0,
                    Wins = 0,
                    Draws = 0,
                    Losses = 0,
                    GoalsScored = 0,
                    GoalsConceded = 0,
                    Points = 0
                };
            }

            var home = table[homeId];
            var away = table[awayId];

            int homeGoals = match.homeScore.Value;
            int awayGoals = match.awayScore.Value;

            home.Played++;
            away.Played++;

            home.GoalsScored += homeGoals;
            home.GoalsConceded += awayGoals;

            away.GoalsScored += awayGoals;
            away.GoalsConceded += homeGoals;

            if (homeGoals > awayGoals)
            {
                home.Wins++;
                home.Points += 3;
                away.Losses++;
            }
            else if (homeGoals < awayGoals)
            {
                away.Wins++;
                away.Points += 3;
                home.Losses++;
            }
            else
            {
                home.Draws++;
                away.Draws++;

                home.Points++;
                away.Points++;
            }
        }

        return table.Values
        .OrderByDescending(t => t.Points)
        .ThenByDescending(t => t.GoalDifference)
        .ToList();
    }

}