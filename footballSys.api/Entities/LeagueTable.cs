
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Table("LeagueTable")]
public class LeagueTable
{
    public int Id { get; set; } 
    public int TeamId { get; set; }

    public int FixtureId { get; set; }

    public int Wins { get; set; }

    public int Draws { get; set; }

    public int Losses { get; set; }

    public int GoalsScored { get; set; }

    public int GoalsConceded { get; set; }

    public int Points { get; set; }

}