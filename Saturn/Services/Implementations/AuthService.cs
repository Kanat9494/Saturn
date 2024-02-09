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

    protected internal static bool IsUserAuthenticated()
    {
        string isUserAuthenticated = "";
        Task.Run(async () =>
        {
            isUserAuthenticated = await SecureStorage.Default.GetAsync("authState") ?? "0";
        }).Wait();

        if (isUserAuthenticated.Equals("1"))
            return true;

        return false;
    }

    protected internal static async Task SignIn(string authState)
    {
        await SecureStorage.Default.SetAsync("authState", "1");

        await Shell.Current.GoToAsync("//MainPage");
    }

    protected internal static async Task SignOut(string authState)
    {
        await SecureStorage.Default.SetAsync("authState", "0");

        await Shell.Current.GoToAsync("//SignInPage");
    }
}
