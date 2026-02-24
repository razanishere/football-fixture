using footballSys.api.Data;
using footballSys.api.Dtos;
using footballSys.api.Endpoints;
using static footballSys.api.Dtos.createTeamDto;

var builder = WebApplication.CreateBuilder(args);

// define a connection between the app and the database
// "footballSys" is the key assigned to the database in the appsettings.json
var connString = builder.Configuration.GetConnectionString("FootballSys");


builder.Services.AddSqlite<teamsContext>(connString);
//builder.Services.AddScoped<teamsContext>()


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); //! this is only for testing. delete later.

    });
});

builder.Services.AddControllers();


var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();
app.UseCors("ReactApp");
app.MapTeamsEndpoints();

await app.MigrateDbAsync();



app.Run();
