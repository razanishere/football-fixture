using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace footballSys.api.Entities;

[Table("Matches")]
public class Match
{
    [Column(Order = 0)]
    public int Id { get; set; } //ID of the match

    [Column(Order = 1)]
    public int HomeTeamId { get; set; }

    [Column(Order = 2)]
    public int AwayTeamId { get; set; }

    [Column(Order = 3)]
    public int Week { get; set; }

    [Column(Order = 4)]
    public int? homeScore {get; set;}

    [Column(Order = 5)]
    public int? awayScore {get; set;}

    [Column(Order = 6)]
    public int isPlayed {get; set;}

    public int fixtureId {get; set;}
}