using System;

using System.ComponentModel.DataAnnotations.Schema;
namespace footballSys.api.Entities;

public class Teams
{
    [Column(Order = 0)]
    public int Id {get; set;}
    
    [Column(Order = 1)]
    public required string teamName {get; set;}

    [Column(Order = 2)]
    public required int yearEstablished {get; set;}

    [Column(Order = 3)]
    public required string teamLogoPath {get; set;}

    [Column(Order = 4)]
    public required string teamColor1 {get; set;}

    [Column(Order = 5)]
    public required string teamColor2 {get; set;}


}
