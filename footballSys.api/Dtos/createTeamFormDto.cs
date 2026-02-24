using System.ComponentModel.DataAnnotations;

namespace footballSys.api.Dtos;

//! DTO for POST controller, multipart form data

public record class CreateTeamFormDto
(
    [Required] string teamName,
    [Required] int yearEstablished,
    [Required] string teamColor1,
    [Required] string teamColor2
);
