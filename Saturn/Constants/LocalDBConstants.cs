namespace Saturn.Constants;

public static class LocalDBConstants
{
    public const string DATABASE_FILENAME = "SaturnLocalDB.db3";

    public const SQLite.SQLiteOpenFlags FLAGS =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        System.IO.Path.Combine(FileSystem.AppDataDirectory, DATABASE_FILENAME);
}
