using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mission06_Henstrom.Models;

public class DesignTimeMovieContextFactory : IDesignTimeDbContextFactory<MovieContext>
{
    public MovieContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MovieContext>();
        optionsBuilder.UseSqlite("Data Source=Movies.sqlite");

        return new MovieContext(optionsBuilder.Options);
    }
}
