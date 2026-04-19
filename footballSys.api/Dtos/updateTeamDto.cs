namespace footballSys.api.Dtos;

public record class updateTeamDto
(
    string teamName, 
    int yearEstablished, 
    //string LogoURL, //! check this for deletion
    string teamColor1,
    string teamColor2
);