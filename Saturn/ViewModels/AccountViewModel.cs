namespace Saturn.ViewModels;

internal class AccountViewModel : BaseViewModel
{
    public AccountViewModel()
    {
        Task.Run(InitializeUserAsync);
        SignOutCommand = new AsyncRelayCommand(OnSignOut);
        Products = new ObservableCollection<Product2>
        {
            new Product2
            {
                Title = "Title "
            },
            new Product2
            {
                Title = "Title 2"
            },
            
        };
    }

    public ICommand SignOutCommand { get; }

    private User _currentUser;
    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }
    public ObservableCollection<Product2> Products { get; set; }
    

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
    }
}
