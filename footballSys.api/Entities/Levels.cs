using System.ComponentModel.DataAnnotations.Schema;
using footballSys.api.Entities;

[Table("TeamLevels")]
public class TeamLevels
{
    public int Id { get; set; }

    
    public int FixtureId { get; set; }
    public Fixture Fixture { get; set; }

    
    public int TeamId { get; set; }
    public Teams Team { get; set; }

    
    public int Level { get; set; } = 5;
}