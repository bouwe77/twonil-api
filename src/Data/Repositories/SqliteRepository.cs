using System;
using System.Globalization;
using System.IO;
using SQLite;

namespace TwoNil.Data.Repositories
{
    public abstract class SqliteRepository
    {
        protected SQLiteConnection Connection;
        protected readonly string _databaseFilePath;

        protected SqliteRepository(string databaseFilePath)
        {
            _databaseFilePath = databaseFilePath;
        }

        public static string Format(DateTime dateTime)
        {
            string timestamp = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            return timestamp;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        protected void Connect()
        {
            if (!File.Exists(_databaseFilePath))
            {
                throw new Exception($"Database '{_databaseFilePath}' not found");
            }

            Connection = new SQLiteConnection(_databaseFilePath, false);
        }

        protected string GetLastModified()
        {
            return Format(DateTime.UtcNow);
        }
    }
}