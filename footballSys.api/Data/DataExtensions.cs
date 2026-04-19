//migrate the database automatically after start up

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace footballSys.api.Data;

public static class DataExtensions
{

    // this method does all the migrations when the app starts,
    // If there are migrations that haven't been applied yet, apply them automatically.
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<teamsContext>();
        await  dbContext.Database.MigrateAsync();
    }

}
