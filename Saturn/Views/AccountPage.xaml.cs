namespace Saturn.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();

        versionText.Text = AppInfo.VersionString;
        firstTab.Background = Color.FromArgb("00cc00");
        secondTab.Background = Colors.Transparent;
        firstTabTitle.TextColor = Color.FromArgb("00cc00");
        secondTabTitle.TextColor = Colors.Gray;

        BindingContext = new AccountViewModel();
	}

    private void OnStartServiceClicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(Platforms.Android.Services.Implementations.ForegroundServiceDemo));
        Android.App.Application.Context.StartForegroundService(intent);
#endif
    }

    void OnPositionChanged(object sender, PositionChangedEventArgs e)
    {
        int currentItemPosition = e.CurrentPosition;
        if (currentItemPosition == 0)
        {
            firstTab.Background = Color.FromArgb("00cc00");
            secondTab.Background = Colors.Transparent;
            firstTabTitle.TextColor = Color.FromArgb("00cc00");
            secondTabTitle.TextColor = Colors.Gray;
        }
        else
        {
            firstTab.Background = Colors.Transparent;
            secondTab.Background = Color.FromArgb("00cc00");
            firstTabTitle.TextColor = Colors.Gray;
            secondTabTitle.TextColor = Color.FromArgb("00cc00");
        }
    }

    private async void OnGoToCustomBottomBarPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CustomBottomBarPage");
    }
}