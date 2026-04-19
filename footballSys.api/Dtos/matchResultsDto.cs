using System.ComponentModel.DataAnnotations;

namespace footballSys.api.Dtos;

public record class matchResultsDto
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public string HomeTeamName { get; set; }
    public string AwayTeamName { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int Week { get; set; }
}