using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mission06_Henstrom.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Director = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<string>(type: "TEXT", nullable: false),
                    Edited = table.Column<bool>(type: "INTEGER", nullable: true),
                    Lent = table.Column<string>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieID);
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MovieID", "Category", "Title", "Year", "Director", "Rating", "Edited", "Lent", "Notes" },
                values: new object[,]
                {
                    { 1, "Horror/Suspense", "28 Days Later", 2002, "Danny Boyle", "R", true, null, "Plex" },
                    { 2, "Drama", "La La Land", 2016, "Damien Chazelle", "PG-13", false, null, "Plex" },
                    { 3, "Action/Adventure", "Star Wars Episode I: The Phantom Menace", 1999, "George Lucas", "PG", false, null, "Plex" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
