namespace footballSys.api.Dtos;

public record class footballTeamIntro(
    int Id,
    string teamName, 
    int yearEstablished, 
    string LogoURL,
    string teamColor1,
    string teamColor2
    );