using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mission06_Henstrom.Migrations
{
    /// <inheritdoc />
    public partial class Mission07CategoryCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.Sql(
                """
                INSERT INTO Categories (CategoryId, CategoryName)
                VALUES
                    (1, 'Action/Adventure'),
                    (2, 'Comedy'),
                    (3, 'Drama'),
                    (4, 'Family'),
                    (5, 'Horror/Suspense'),
                    (6, 'Miscellaneous'),
                    (7, 'Television'),
                    (8, 'VHS'),
                    (9, 'Uncategorized');
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO Categories (CategoryName)
                SELECT DISTINCT category_name
                FROM (
                    SELECT COALESCE(NULLIF(TRIM(Category), ''), 'Uncategorized') AS category_name
                    FROM Movies
                ) AS source_categories
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Categories c
                    WHERE c.CategoryName = source_categories.category_name
                );
                """);

            migrationBuilder.Sql(
                """
                CREATE TABLE Movies_New (
                    MovieId INTEGER NOT NULL CONSTRAINT PK_Movies PRIMARY KEY AUTOINCREMENT,
                    CategoryId INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    Year INTEGER NOT NULL,
                    Director TEXT NULL,
                    Rating TEXT NULL,
                    Edited INTEGER NOT NULL,
                    LentTo TEXT NULL,
                    CopiedToPlex INTEGER NOT NULL,
                    Notes TEXT NULL,
                    CONSTRAINT FK_Movies_Categories_CategoryId FOREIGN KEY (CategoryId) REFERENCES Categories (CategoryId) ON DELETE RESTRICT
                );
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO Movies_New (MovieId, CategoryId, Title, Year, Director, Rating, Edited, LentTo, CopiedToPlex, Notes)
                SELECT
                    m.MovieID,
                    COALESCE(c.CategoryId, (SELECT CategoryId FROM Categories WHERE CategoryName = 'Uncategorized' LIMIT 1)),
                    m.Title,
                    m.Year,
                    m.Director,
                    m.Rating,
                    COALESCE(m.Edited, 0),
                    m.Lent,
                    CASE
                        WHEN LOWER(COALESCE(m.Notes, '')) = 'plex' THEN 1
                        ELSE 0
                    END,
                    m.Notes
                FROM Movies m
                LEFT JOIN Categories c
                    ON c.CategoryName = COALESCE(NULLIF(TRIM(m.Category), ''), 'Uncategorized');
                """);

            migrationBuilder.Sql(
                """
                DROP TABLE Movies;
                ALTER TABLE Movies_New RENAME TO Movies;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CategoryId",
                table: "Movies",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE TABLE Movies_Old (
                    MovieID INTEGER NOT NULL CONSTRAINT PK_Movies PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    Title TEXT NOT NULL,
                    Year INTEGER NOT NULL,
                    Director TEXT NOT NULL,
                    Rating TEXT NOT NULL,
                    Edited INTEGER NULL,
                    Lent TEXT NULL,
                    Notes TEXT NULL
                );
                """);

            migrationBuilder.Sql(
                """
                INSERT INTO Movies_Old (MovieID, Category, Title, Year, Director, Rating, Edited, Lent, Notes)
                SELECT
                    m.MovieId,
                    COALESCE(c.CategoryName, 'Uncategorized'),
                    m.Title,
                    m.Year,
                    COALESCE(m.Director, ''),
                    COALESCE(m.Rating, ''),
                    m.Edited,
                    m.LentTo,
                    CASE
                        WHEN m.CopiedToPlex = 1 AND (m.Notes IS NULL OR TRIM(m.Notes) = '') THEN 'Plex'
                        ELSE m.Notes
                    END
                FROM Movies m
                LEFT JOIN Categories c
                    ON c.CategoryId = m.CategoryId;
                """);

            migrationBuilder.Sql(
                """
                DROP TABLE Movies;
                ALTER TABLE Movies_Old RENAME TO Movies;
                """);

            migrationBuilder.DropTable(name: "Categories");
        }
    }
}
