using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MinesweeperML.Business.Constants
{
    /// <summary>
    /// The constant strings.
    /// </summary>
    public static class StringConstants
    {
        /// <summary>
        /// The minesweeper database connection string.
        /// </summary>
        public static string MinesweeperDbConnectionString = ConfigurationBuilderHelper.GetConfiguration().GetConnectionString("MinesweeperDatabase");

        /// <summary>
        /// The minesweeper database path.
        /// </summary>
        public static string MinesweeperDbPath = @"data.db";
    }
}