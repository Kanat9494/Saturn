namespace Saturn.Services.Implementations;

internal class AuthService
{
    internal AuthService()
    {

    }

    private static AuthService _instance;
    public static AuthService GetInstance()
    {
        if (_instance == null)
            _instance = new AuthService();

        return _instance;
    }

    protected internal bool IsUserAuthenticated()
    {
        string isUserAuthenticated = "";
        Task.Run(async () =>
        {
            isUserAuthenticated = await SecureStorage.Default.GetAsync("isUserAuthenticated") ?? "0";
        }).Wait();

        if (isUserAuthenticated.Equals("1"))
            return true;

        return false;
    }
}
