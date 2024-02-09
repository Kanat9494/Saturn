namespace Saturn.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();

        BindingContext = new SignInViewModel();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (AuthService.GetInstance().IsUserAuthenticated())
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        
    }
}