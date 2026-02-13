using Microsoft.EntityFrameworkCore;
using Mission06_Henstrom.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MovieContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:MoviesConnection"]);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
    SeedMigrationHistoryForExistingDatabase(db);
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

static void SeedMigrationHistoryForExistingDatabase(MovieContext db)
{
    db.Database.OpenConnection();

    try
    {
        if (!TableExists(db, "Movies"))
        {
            return;
        }

        ExecuteNonQuery(db,
            """
            CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
                "ProductVersion" TEXT NOT NULL
            );
            """);

        var migrationCount = ExecuteScalarInt(db, """SELECT COUNT(*) FROM "__EFMigrationsHistory";""");
        if (migrationCount > 0)
        {
            return;
        }

        var hasCategoriesTable = TableExists(db, "Categories");
        var hasCategoryIdColumn = ColumnExists(db, "Movies", "CategoryId");
        var hasCopiedToPlexColumn = ColumnExists(db, "Movies", "CopiedToPlex");

        // Baseline legacy Mission 6 databases so EF does not try to recreate Movies.
        ExecuteNonQuery(db,
            """
            INSERT OR IGNORE INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20260209203014_Initial', '10.0.2');
            """);

        // If the database is already at Mission 07 schema, baseline that migration too.
        if (hasCategoriesTable && hasCategoryIdColumn && hasCopiedToPlexColumn)
        {
            ExecuteNonQuery(db,
                """
                INSERT OR IGNORE INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
                VALUES ('20260213035332_Mission07CategoryCrud', '10.0.2');
                """);
        }
    }
    finally
    {
        db.Database.CloseConnection();
    }
}

static bool TableExists(MovieContext db, string tableName)
{
    var command = db.Database.GetDbConnection().CreateCommand();
    command.CommandText = """SELECT COUNT(*) FROM "sqlite_master" WHERE "type" = 'table' AND "name" = $name;""";
    var parameter = command.CreateParameter();
    parameter.ParameterName = "$name";
    parameter.Value = tableName;
    command.Parameters.Add(parameter);

    return Convert.ToInt32(command.ExecuteScalar()) > 0;
}

static bool ColumnExists(MovieContext db, string tableName, string columnName)
{
    var command = db.Database.GetDbConnection().CreateCommand();
    command.CommandText = $"PRAGMA table_info(\"{tableName}\");";

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        if (reader.GetString(1).Equals(columnName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
    }

    return false;
}

static int ExecuteScalarInt(MovieContext db, string sql)
{
    var command = db.Database.GetDbConnection().CreateCommand();
    command.CommandText = sql;
    return Convert.ToInt32(command.ExecuteScalar());
}

static void ExecuteNonQuery(MovieContext db, string sql)
{
    var command = db.Database.GetDbConnection().CreateCommand();
    command.CommandText = sql;
    command.ExecuteNonQuery();
}
