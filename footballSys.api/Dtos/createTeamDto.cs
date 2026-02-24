using System.ComponentModel.DataAnnotations;

namespace footballSys.api.Dtos;

public record class createTeamDto
(
    [Required] string teamName, 
    [Required] int yearEstablished, 
    [Required] string LogoURL,
    [Required] string teamColor1,
    [Required] string teamColor2
);

// i am going to get the info from the client so i wont let it give me an id,
// instead i will let my code to it automatically
