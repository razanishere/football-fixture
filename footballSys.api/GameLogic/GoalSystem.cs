/* CODE FLOW EXPLAINED

1- main method: SimulateMatch.

SimulateMatch works on the strength factors: scoring probabilty and no of attempts to score.
it takes the first row of the Match entity (table) and matches the ID's.
then it calculates the attempts of home team and away team using the GenerateAttempts method.
then it maps the probabilty (fixed number based on level) from the StrengthRule.getGoalProbability method
then it calculates the goals using the GenerateGoals method.

it maps the goals to the score field of the Match entity for taken rows.

lastly, it updates the levels of the teams based on the system.

2- GenerateAttempts method:
Generates the number of scoring attempts in the specified range of attempts based on level. (in StrengthRule)

3- GenerateGoals method:
Generates number of SCORED GOALS (in the given attempts number) based on a fixed probability based on level .
the Random generated double numbers in ranges between 0.00 and 0.99, and the ones below the probabilty number is considered a goal.


*/


/* SCORE SYSTEM EXPLAINED

*the initial scoring system will be divided into two factors: 1- chance of scoring 2- range of attempts to score
*the system is a leveled system which each team will level up or level down based on the final result of each match. 
*the level influences the two strength factors. and all teams start with level 5.

*1- chance of scoring: 

Strength	Goal Probability
1	            15%
2	            18%
3	            21%
4	            24%
5	            27%
6	            30%
7	            33%
8	            36%
9	            40%

*2- range of attempts to score:
Strength	Attempt Range
1	            2 – 5
2	            3 – 6
3	            4 – 7
4	            5 – 8
5	            6 – 9
6	            7 – 10
7	            8 – 11
8	            9 – 12
9	            10 – 13

*the range is fixed, but the actual number of attempts is RANDOM. which assurs the unpredictability the instructor asked from me.
*after each match, the score in total gets calculated. the winner team +1 level, loser -1 level and draw dosent chamge levels.

so the flow will be like this:
*Strength > Attempts > Goals > Match Result > Points > Strength Update

*/


using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using footballSys.api.Data;
using footballSys.api.Entities;

public class GoalSystem
{

    private readonly teamsContext _context;

    public GoalSystem(teamsContext context)
    {
        _context = context;
    }



    //* method to generate attempts 

    private int GenerateAttempts(int level, Random rnd)
    {
        int minRange = StrengthRule.getMinAttempts(level);
        int maxRange = StrengthRule.getMaxAttempts(level);

        return rnd.Next(minRange, maxRange + 1);

    }

    //* method to generate goals

    private int GenerateGoals(int attempts, double probability, Random rnd)
    {
        int goals = 0;

        for (int i = 0; i < attempts; i++)
        {
            if (rnd.NextDouble() < probability)
            {
                goals++;
            }
        }

        return goals;

    }

    private void UpdateLevel(int homeGoals, int awayGoals, Teams home, Teams away)
{
    int diff = Math.Abs(homeGoals - awayGoals);

    
    if (diff > 1)
    {
        if (homeGoals > awayGoals)
        {
            home.level++;
            away.level--;
        }
        else
        {
            away.level++;
            home.level--;
        }
    }

    home.level = Math.Clamp(home.level, 1, 9);
    away.level = Math.Clamp(away.level, 1, 9);
}

    

    // we dont save to database here because this method gets used continuesly and the data keeps getting updated
    // we save the scores to the database in the ScoreGenerator method
    //* the match 
    public void SimulateMatch(Match match, Random rnd)
    {
        var homeTeam = _context.Teams.First(t => t.Id == match.HomeTeamId);
        var awayTeam = _context.Teams.First(t => t.Id == match.AwayTeamId);

        int homeAttempts = GenerateAttempts(homeTeam.level, rnd);
        double homeProbability = StrengthRule.getGoalProbability(homeTeam.level);
        int homeGoals = GenerateGoals(homeAttempts, homeProbability, rnd);

        int awayAttempts = GenerateAttempts(awayTeam.level, rnd);
        double awayProbability = StrengthRule.getGoalProbability(awayTeam.level);
        int awayGoals = GenerateGoals(awayAttempts, awayProbability, rnd);

        match.homeScore = homeGoals;
        match.awayScore = awayGoals;


       UpdateLevel(homeGoals, awayGoals, homeTeam, awayTeam);
        


    }


}