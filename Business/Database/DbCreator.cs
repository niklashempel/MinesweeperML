using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MinesweeperML.Business;
using MinesweeperML.Business.Constants;

namespace MinesweeperML.Business.Database
{
    /// <summary>
    /// Helper to create the database file.
    /// </summary>
    public static class DbCreator
    {
        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        public static async Task CreateAsync()
        {
            using var con = new SqliteConnection(StringConstants.MinesweeperDbConnectionString);
            await con.OpenAsync();

            using var cmd = new SqliteCommand(string.Empty, con);
            cmd.CommandText = "DROP TABLE IF EXISTS Highscores";
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = @"CREATE TABLE Highscores(id INTEGER PRIMARY KEY, difficulty INTEGER, time BIGINTEGER)";
            await cmd.ExecuteNonQueryAsync();
        }
    }
}