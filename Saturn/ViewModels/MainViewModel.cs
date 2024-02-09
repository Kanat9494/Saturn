namespace Saturn.ViewModels;

internal class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        Products = new ObservableCollection<Product>();
        IsBusy = true;

        Task.Run(async () =>
        {
            _userId = await SecureStorage.Default.GetAsync("userId");
            await GenerateProducts();
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
