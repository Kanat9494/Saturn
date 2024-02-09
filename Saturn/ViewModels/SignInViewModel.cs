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
        await AuthService.SignIn("1");
    }
}
