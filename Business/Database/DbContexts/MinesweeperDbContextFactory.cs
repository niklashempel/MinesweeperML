using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MinesweeperML.Business.Constants;

namespace MinesweeperML.Business.Database.DbContexts
{
    /// <summary>
    /// Provides a factory to access the minesweeper database.
    /// </summary>
    /// <seealso cref="IDesignTimeDbContextFactory{MinesweeperDbContext}" />
    public class MinesweeperDbContextFactory : IDesignTimeDbContextFactory<MinesweeperDbContext>
    {
        /// <summary>
        /// Creates the database context.
        /// </summary>
        public MinesweeperDbContext CreateDbContext()
        {
            return this.CreateDbContext(null);
        }

        /// <summary>
        /// Creates a new instance of a derived context.
        /// </summary>
        /// <param name="args">Arguments provided by the design-time service.</param>
        /// <returns>An instance of <see cref="MinesweeperDbContext" />.</returns>
        public MinesweeperDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MinesweeperDbContext>();
            optionsBuilder.UseSqlite(StringConstants.MinesweeperDbConnectionString);

            return new MinesweeperDbContext(optionsBuilder.Options);
        }
    }
}