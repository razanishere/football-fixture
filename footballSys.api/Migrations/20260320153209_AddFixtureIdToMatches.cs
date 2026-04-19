using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace footballSys.api.Migrations
{
    /// <inheritdoc />
    public partial class AddFixtureIdToMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fixtureId",
                table: "Matches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fixtureId",
                table: "Matches");
        }
    }
}
