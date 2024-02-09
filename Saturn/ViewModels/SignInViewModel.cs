namespace Saturn.ViewModels;

internal class SignInViewModel : BaseViewModel
{
    public SignInViewModel()
    {
        SignInCommand = new AsyncRelayCommand(OnSignIn);
    }

    public ICommand SignInCommand { get; }

    private ulong? _userId;
    public ulong? UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    private async Task OnSignIn()
    {
        IsBusy = true;
        await Task.Delay(3000);
        if (UserId == null || UserId == 0)
        {
            IsBusy = false;
            return;

        }
        await AuthService.SignIn("1", UserId.ToString());
        IsBusy = false;
    }
}
