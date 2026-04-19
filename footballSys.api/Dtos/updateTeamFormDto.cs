using Microsoft.AspNetCore.Mvc;

namespace footballSys.api.Dtos;

public class UpdateTeamFormDto
{
    [FromForm]
    public string? teamName { get; set; }

    [FromForm]
    public int yearEstablished { get; set; }

    [FromForm]
    public string? teamColor1 { get; set; }

    [FromForm]
    public string? teamColor2 { get; set; }
}