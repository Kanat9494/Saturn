namespace Saturn.ViewModels;

internal class AccountViewModel : BaseViewModel
{
    public AccountViewModel()
    {
        Task.Run(async () =>
        {
            await InitializeUserAsync();
            await InitializeUserBlogs();
        });
        SignOutCommand = new AsyncRelayCommand(OnSignOut);
        var firstTabItems = new List<BlogPost>();
        var secondTabItems = new List<BlogPost>();
        Tabs = new ObservableCollection<CustomTab>()
        {
            new CustomTab
            {
                TabItems = firstTabItems
            },
            new CustomTab
            {
                TabItems = secondTabItems
            }
        };
    }

    public ICommand SignOutCommand { get; }

    private User _currentUser;
    public User CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }
    public ObservableCollection<CustomTab> Tabs { get; set; }

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

    async Task InitializeUserBlogs()
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        for (int i = 1; i <= 13; i++)
        {
            if (i % 2 == 0)
            {
                Tabs[0].TabItems.Add(new BlogPost(i, "Lorem Ipsum asdf;lasdf", "https://picsum.photos/id/237/200/300", $"https://picsum.photos/id/{i}/200/300",
                    "Lasdfl asdlfk Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's st"));
            }
            else
            {
                Tabs[1].TabItems.Add(new BlogPost(i, "Lorem Ipsum asdf;lasdf", "https://picsum.photos/id/237/200/300", $"https://picsum.photos/id/{i}/200/300",
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker includ"));
            }
        }
    }
}
