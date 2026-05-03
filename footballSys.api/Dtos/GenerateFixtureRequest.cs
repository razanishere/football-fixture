public class GenerateFixtureRequest
{
    public List<int> TeamIds { get; set; }
    public string FixtureName { get; set; }

    public bool IsSpecialMode { get; set; }
}

// this DTO is for "generate" endpoint recieves.