using System;
using footballSys.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace footballSys.api.Data;


//* file name is teamsContext but i will be adding all the other contexts here.

public class teamsContext(DbContextOptions<teamsContext> options) : DbContext(options)
{

    // telling the system: "Create a table in the database based on the Teams entity.”
    public DbSet<Teams> Teams => Set<Teams>();

    public DbSet<Match> Matches => Set<Match>();

    public DbSet<LeagueTable> LeagueTable => Set<LeagueTable>();

}
