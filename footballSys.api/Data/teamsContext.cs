using System;
using footballSys.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace footballSys.api.Data;

public class teamsContext(DbContextOptions<teamsContext> options) : DbContext(options)
{

    // telling the system: "I want a table in the database that looks exactly like my Teams entity
    public DbSet<Teams> Teams => Set<Teams>();
}
