using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace footballSys.api.Migrations
{
    /// <inheritdoc />
    public partial class LinkMatchToFixture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Matches_fixtureId",
                table: "Matches",
                column: "fixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Fixtures_fixtureId",
                table: "Matches",
                column: "fixtureId",
                principalTable: "Fixtures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Fixtures_fixtureId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_fixtureId",
                table: "Matches");
        }
    }
}
