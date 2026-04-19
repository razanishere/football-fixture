using System;

//! i used XML comments here.

/*
lets say we have 4 teams
in week 1, the play is going to be like this: A vs B and C vs D
in week 2, A vs C and B vs D

and so on

so in this example, a round is this : , A vs C and B vs D
and a match is this: A vs C
*/

namespace footballSys.api.Services
{
    // This class handles all calculations for fixture generation
    public class FixtureCalculations
    {
        /// <summary>
        /// Returns the total number of fixtures for the tournament
        /// </summary>
        /// <param name="fixtureCountPerMeeting">Number of fixtures per round</param>
        /// <param name="meetingCount">Number of rounds</param>
        /// <returns>Total fixture count</returns>
        public int GetTotalFixtureCount(int fixtureCountPerMeeting, int meetingCount)
        {
            return fixtureCountPerMeeting * meetingCount;
        }

        /// <summary>
        /// Returns the number of fixtures (matches) in a single round
        /// </summary>
        /// <param name="teamCount">Number of teams</param>
        /// <returns>Number of matches per round</returns>
        public int GetFixtureCountPerMeeting(int teamCount)
        {
            // (teamCount * (teamCount - 1)) / 2
            return (teamCount * (teamCount - 1)) / 2;
        }

        /// <summary>
        /// Returns the number of rounds based on team count
        /// Even number of teams: rounds = teamCount - 1
        /// Odd number of teams: rounds = teamCount
        /// </summary>
        /// <param name="teamCount">Number of teams</param>
        /// <returns>Number of rounds</returns>
        public int GetRoundCount(int teamCount)
        {
            return (teamCount % 2 == 0) ? (teamCount - 1) : teamCount;
        }
    }
}