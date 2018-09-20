using System;
using System.IO;
using SQLite;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data
{
    internal class GameDatabaseManager
    {
        private string _databaseFilePath;

        internal GameDatabaseManager(string databaseFilePath)
        {
            _databaseFilePath = databaseFilePath;
        }

        public void Create()
        {
            if (File.Exists(_databaseFilePath))
            {
                throw new Exception($"Database '{_databaseFilePath}' already exists");
            }

            using (var connection = new SQLiteConnection(_databaseFilePath, false))
            {
                connection.CreateTable<GameInfo>();
                connection.CreateTable<Player>();
                connection.CreateTable<LeagueTable>();
                connection.CreateTable<LeagueTablePosition>();
                connection.CreateTable<Round>();
                connection.CreateTable<Match>();
                connection.CreateTable<Team>();
                connection.CreateTable<Season>();
                connection.CreateTable<SeasonCompetition>();
                connection.CreateTable<SeasonCompetitionTeam>();
                connection.CreateTable<SeasonStatistics>();
                connection.CreateTable<SeasonTeamStatistics>();
                connection.CreateTable<TeamStatistics>();
                connection.CreateTable<GameDateTime>();
            }
        }

        public void Delete()
        {
            File.Delete(_databaseFilePath);
        }
    }
}
