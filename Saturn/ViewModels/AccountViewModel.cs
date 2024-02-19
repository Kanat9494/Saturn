namespace Saturn.ViewModels;

internal class AccountViewModel : BaseViewModel
{
    public AccountViewModel()
    {
        Task.Run(InitializeUserAsync);
        SignOutCommand = new AsyncRelayCommand(OnSignOut);
    }

    public ICommand SignOutCommand { get; }

    private User _currentUser;
    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }
    private string _imageUrl;
    public string ImageUrl
    {
        get => _imageUrl;
        set => SetProperty(ref _imageUrl, value);
    }
    

    private async Task OnSignOut()
    {
        await AuthService.SignOut("0");
    }

    async Task InitializeUserAsync()
    {
        var userId = await SecureStorage.Default.GetAsync("userId");

        CurrentUser = new User
        {
            UserId = Convert.ToUInt64(userId),
            UserName = "Кудайбергенов Канат Кудайбергенович",
            ProfileImageSource = "https://picsum.photos/id/301/200/300"
        };

        ImageUrl = null;
        ImageUrl = "https://picsum.photos/id/301/200/300";
    }
}
