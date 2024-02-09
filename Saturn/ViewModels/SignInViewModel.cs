namespace Saturn.ViewModels;

internal class SignInViewModel : BaseViewModel
{
    public SignInViewModel()
    {
        SignInCommand = new AsyncRelayCommand(OnSignIn);
    }

    public ICommand SignInCommand { get; }

    private async Task OnSignIn()
    {
        await SecureStorage.Default.SetAsync("authState", "1");

        await Shell.Current.GoToAsync("//MainPage");
    }
}
