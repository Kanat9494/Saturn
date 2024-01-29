namespace Saturn.Constants;

internal static class LocalDBConstants
{
    internal const string DATABASE_FILENAME = "LocalDB.db3";
    internal const SQLite.SQLiteOpenFlags FLAGS =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    internal static string DatabasePath => System.IO.Path.Combine(FileSystem.AppDataDirectory, DATABASE_FILENAME);
}
