namespace Saturn.ViewModel;

internal class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        Products = new ObservableCollection<Product>();

        //Task.Run(async () =>
        //{
        //    await GenerateProducts();
        //}).GetAwaiter().OnCompleted(() =>
        //{
        //    IsBusy = false;
        //});
    }

    public ObservableCollection<Product> Products { get; set; }

    internal async Task GenerateProducts()
    {
        await Task.Delay(3000);
        for (int i = 0; i < 500; i++)
        {
            Products.Add(new Product
            {
                ProductId = i,
                ProductName = "asdfl asdlfk",
                ImageUrl = "https://picsum.photos/200/300",
                CategoryId = 1,
                Price = 500.0
            });
        }
    }
}
