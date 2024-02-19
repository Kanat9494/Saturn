namespace Saturn.Views;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();

        activityBorder.BackgroundColor = Color.FromRgba(0, 0, 0, 160);

        BindingContext = new SignInViewModel();
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (AuthService.IsUserAuthenticated())
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

    }
}