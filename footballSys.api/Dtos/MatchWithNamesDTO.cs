namespace footballSys.api.Data
{
    
    public class MatchWithNamesDTO
    {
        public int HomeTeamId {get; set;}
        public int AwayTeamId {get; set;}
        public string HomeTeamName {get; set;}
        public string AwayTeamName {get; set;}
        public int week {get; set;}

    }

}