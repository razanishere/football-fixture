using footballSys.api.Data;

//! understand this code
public class ScoreGenerator
{
    private readonly teamsContext _context;
    private readonly GoalSystem _goalSystem;

    public ScoreGenerator(teamsContext context)
    {
        _context = context;
        _goalSystem = new GoalSystem(context);
    }


    //* Generate scores for ALL matches/rounds
    public void GenerateScore(int fixtureId)
    {

        var matches = _context.Matches
        .Where(m => m.fixtureId == fixtureId)
        .ToList();


        Random rnd = new Random();

        foreach (var match in matches)
        {
            
            if (match.homeScore != null || match.awayScore != null)
                continue;

            _goalSystem.SimulateMatch(match, rnd);
        }

        Console.WriteLine($"Matches found: {matches.Count}");
        _context.SaveChanges();
    }


    //* this is only for one week
    public void GenerateScoreForWeek( int fixtureId, int week)
    {
        var matches = _context.Matches
        .Where(m => m.fixtureId == fixtureId && m.Week == week)
        .ToList();


        Random rnd = new Random();

        foreach(var match in matches)
        {
            if(match.homeScore != null || match.awayScore != null)
            continue;

            _goalSystem.SimulateMatch(match, rnd);

        }

        _context.SaveChanges();
        
    }


}