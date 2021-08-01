using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinesweeperML.Models;

namespace MinesweeperML.Business.Database.DbContexts
{
    /// <summary>
    /// The minesweeper database context.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class MinesweeperDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the high scores.
        /// </summary>
        /// <value>The high scores.</value>
        public DbSet<Highscore> HighScores { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinesweeperDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MinesweeperDbContext(DbContextOptions<MinesweeperDbContext> options)
            : base(options)
        {
        }
    }
}