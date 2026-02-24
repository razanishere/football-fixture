//migrate the database automatically after start up

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace footballSys.api.Data;

public static class DataExtensions
{

    //! understand: why use a task, why make it async ???
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<teamsContext>();
        await  dbContext.Database.MigrateAsync();
    }

}
