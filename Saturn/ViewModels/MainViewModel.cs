﻿namespace Saturn.ViewModels;

public class MainViewModel : BaseViewModel
{
    public MainViewModel(IServiceProvider serviceProvider)
    {
        Products = new ObservableCollection<Product>();
        Blogs = new ObservableCollection<BlogPost>();
        IsBusy = true;
        ShareCommand = new AsyncRelayCommand<int>(OnShareUri);
        RefreshPageCommand = new AsyncRelayCommand(InitializeBlogs);
        try
        {
            _clientWSManager = serviceProvider.GetService<ClientWSManager>();

        }
        catch (Exception ex)
        {

        }
        Task.Run(async () =>
        {
            _userId = await SecureStorage.Default.GetAsync("userId");
            await InitializeBlogs();
        }).GetAwaiter().OnCompleted(async () =>
        {
            IsBusy = false;
            if (_clientWSManager._isConnected)
            {
                await _clientWSManager.DisconnectAsync();
                _clientWSManager?.ConnectToWSServer(ulong.Parse(_userId ?? "0"));
            }
            else
            {
                _clientWSManager?.ConnectToWSServer(ulong.Parse(_userId ?? "0"));
            }
            AuthFields.UserId = int.Parse(_userId ?? "0");
        });

        //RTServerManager.ConnectToRTCServer(1, 54);

    }

    public ICommand ShareCommand { get; }
    public ICommand RefreshPageCommand { get; }

    private ClientWSManager _clientWSManager;


    private string _userId;

    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<BlogPost> Blogs { get; set; }



    private bool _isContent;
    public bool IsContent
    {
        get => _isContent;
        set => SetProperty(ref _isContent, value);
    }
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
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
        Blogs.Clear();
        //IsBusy = true;
        //IsContent = false;
        await Task.Delay(3000);
        for (int i = 1; i <= 300; i++)
        {
            if (i % 2 == 0)
                Blogs.Add(new BlogPost(i, "Lorem Ipsum asdf;lasdf", "https://picsum.photos/id/237/200/300", $"https://picsum.photos/id/{i}/200/300",
                    "Lasdfl asdlfk Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's st"));
            else
                Blogs.Add(new BlogPost(i, "Lorem Ipsum asdf;lasdf", "https://picsum.photos/id/237/200/300", $"https://picsum.photos/id/{i}/200/300",
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker includ"));
        }

        IsBusy = false;
        IsContent = true;
        IsRefreshing = false;
    }

    private async Task OnShareUri(int blogId)
    {
        string uri = $"{ServerConstants.DEEPLINK_DATA_SCHEME}://{ServerConstants.DEEPLINK_DATA_HOST}/{ServerConstants.DEEPLINK_DATA_PATH_PREFIX}/{blogId}";
        await Share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Поделиться постом"
        });
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
