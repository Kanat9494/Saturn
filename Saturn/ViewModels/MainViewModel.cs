namespace Saturn.ViewModels;

internal class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        Products = new ObservableCollection<Product>();
        Blogs = new ObservableCollection<BlogPost>();
        IsBusy = true;

        Task.Run(async () =>
        {
            _userId = await SecureStorage.Default.GetAsync("userId");
            await InitializeBlogs();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsBusy = false;
        });

        //RTServerManager.ConnectToRTCServer(1, 54);
        ClientWSManager.ConnectToWSServer(ulong.Parse(_userId ?? "0"));
        //RTServerManager.ConnectToRTCServer(1, 54);

    }

    private string _userId;

    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<BlogPost> Blogs { get; set; }



    private bool _isContent;
    public bool IsContent
    {
        get => _isContent;
        set => SetProperty(ref _isContent, value);
    }

    internal async Task GenerateProducts()
    {
        await Task.Delay(3000);
        for (int i = 0; i < 500; i++)
        {
            Products.Add(new Product
            {
                ProductId = i,
                ProductName = "asdfl asdlfk Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's st",
                ImageUrl = "https://picsum.photos/200/300",
                CategoryId = 1,
                Price = 500.0
            });
        }
        IsBusy = false;
        IsContent = true;
    }

    internal async Task InitializeBlogs()
    {
        await Task.Delay(3000);
        for (int i = 1; i <= 50; i++)
        {
            Blogs.Add(new BlogPost(i, "Lorem Ipsum asdf;lasdf", "https://picsum.photos/id/237/200/300", $"https://picsum.photos/id/{i}/200/300",
                "asdfl asdlfk Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's st"));
        }

        IsBusy = false;
        IsContent = true;
    }

    internal async void LaunchApp()
    {
        try
        {
            OpenOtherApp openOtherApp = new OpenOtherApp();
            openOtherApp.LaunchApp("com.maanavan.mb_kyrgyzstan");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "Ок");
        }
    }
}
