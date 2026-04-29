using System.ComponentModel.DataAnnotations.Schema;

[Table("Fixtures")]
public class Fixture
{
    public int Id { get; set; }
    public bool IsFinished { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Name { get; set; }

}