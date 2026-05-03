namespace footballSys.api.Services;

using footballSys.api.Data;
using footballSys.api.DTOs;
using footballSys.api.Entities;
public class FixtureGenerator
{

    //injecting db context for adding results to db
    private readonly teamsContext _context;

    // Add this constructor after the field declarations
    public FixtureGenerator(teamsContext context)
    {
        _context = context;
    }

    private FixtureCalculations _calculations = new FixtureCalculations();

    private int _teamCountOriginal;
    private int _roundCount;
    private int _fixtureCountPerMeeting;
    private int _totalFixtureCount;

    // Variables
    private int _meetingsCount; // holds the number of round robin
    private const int DUMMY_TEAM = 0;


    private List<int> _teams = new List<int>();
    private bool _teamSet = false; //are the teams ready to use?

    // team initialization
    public void InitializeFixtureSettings(List<int> selectedTeams)
    {
        _teams = new List<int>(selectedTeams);

        _teamCountOriginal = _teams.Count;


        // if odd number of teams add dummy team
        if (_teams.Count % 2 == 1)
        {
            _teams.Add(0);
        }

        _roundCount = _calculations.GetRoundCount(_teamCountOriginal);

        _fixtureCountPerMeeting = _calculations.GetFixtureCountPerMeeting(_teamCountOriginal);

        _totalFixtureCount = _calculations.GetTotalFixtureCount(
            _fixtureCountPerMeeting,
            _roundCount
        );

        _teamSet = true;
    }

    
    private void InitializeFixtureTeamLevels(int fixtureId)
{
    var teamIds = _teams.Where(t => t != DUMMY_TEAM).ToList();

    foreach (var teamId in teamIds)
    {
        var levelEntry = new TeamLevels
        {
            FixtureId = fixtureId,
            TeamId = teamId,
            Level = 5
        };

        _context.TeamLevels.Add(levelEntry);
    }

    _context.SaveChanges();
}
    
    
    
    //? look at this 
    private List<MatchDTO> CopyRoundToMainFixtures(int roundNumber)
    {
        var roundMatches = new List<MatchDTO>();

        int lastTeamPosition = _teams.Count - 1;

        for (int i = 0; i < _teams.Count / 2; i++)
        {
            int team1 = _teams[i];
            int team2 = _teams[lastTeamPosition - i];

            if (!CheckForDummyTeam(team1, team2))
            {
                roundMatches.Add(new MatchDTO
                {
                    HomeTeamId = team1,
                    AwayTeamId = team2,
                    Week = roundNumber
                });
            }
        }


        return roundMatches;
    }
    private void RotateTeams()
    {
        int rotateToPosition = 2;

        // store last team
        //* temp here is used to hold the last team in the list before we shift them all back
        int temp = _teams[_teams.Count - 1];

        // shift teams backwards
        for (int i = _teams.Count - 1; i >= rotateToPosition; i--)
        {
            _teams[i] = _teams[i - 1];
        }

        // move last team to position 1 (index 1)
        _teams[1] = temp;
    }

    private bool CheckForDummyTeam(int team1, int team2)
    {
        return team1 == DUMMY_TEAM || team2 == DUMMY_TEAM;
    }

    private List<List<MatchDTO>> GenerateReverseFixtures(List<List<MatchDTO>> fixtures)
    {
        var reverseFixtures = new List<List<MatchDTO>>();

        foreach (var week in fixtures)
        {
            var reverseWeek = new List<MatchDTO>();

            foreach (var match in week)
            {
                reverseWeek.Add(new MatchDTO
                {

                    HomeTeamId = match.AwayTeamId,
                    AwayTeamId = match.HomeTeamId,
                    Week = match.Week + _roundCount // Schedule reverse fixtures after the initial rounds
                });
            }

            reverseFixtures.Add(reverseWeek);
        }

        return reverseFixtures;
    }


    //method to save fixture result to the database
    private int SaveFixtureToDB(List<List<MatchDTO>> fixtures, int fixtureId)
    {
        foreach (var week in fixtures)
        {
            foreach (var matchDTO in week)
            {
                var matchEntity = new Match
                {
                    HomeTeamId = matchDTO.HomeTeamId,
                    AwayTeamId = matchDTO.AwayTeamId,
                    Week = matchDTO.Week,
                    fixtureId = fixtureId,
                    homeScore = null,
                    awayScore = null,
                    isPlayed = 0
                };

                _context.Matches.Add(matchEntity);
            }
        }

        _context.SaveChanges();

        return fixtureId;
    }


    public (int fixtureId, List<List<MatchDTO>> fixtures) CreateFixtures(string fixtureName)
    {
        var fixtures = new List<List<MatchDTO>>();

        for (int i = 1; i <= _roundCount; i++)
        {
            var roundMatches = CopyRoundToMainFixtures(i);
            fixtures.Add(roundMatches);

            RotateTeams();
        }

        var reverseFixtures = GenerateReverseFixtures(fixtures);
        fixtures.AddRange(reverseFixtures);

        //! isFinished will change
        var fixtureEntity = new Fixture
        {
            Name = fixtureName,
            CreatedAt = DateTime.Now,
            IsFinished = false
        };

        _context.Fixtures.Add(fixtureEntity);
        _context.SaveChanges();

        int fixtureId = fixtureEntity.Id;


        InitializeFixtureTeamLevels(fixtureId);

        SaveFixtureToDB(fixtures, fixtureId);

        return (fixtureId, fixtures);
    }


}