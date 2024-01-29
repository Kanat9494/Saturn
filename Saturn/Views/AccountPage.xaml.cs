namespace Saturn.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
	}

    private void OnStartServiceClicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.Content.Intent intent = new Android.Content.Intent(Android.App.Application.Context, typeof(Platforms.Android.Services.Implementations.ForegroundServiceDemo));
        Android.App.Application.Context.StartForegroundService(intent);
#endif
    }
}