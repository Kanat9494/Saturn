namespace Saturn.ViewModels;

internal class AccountViewModel : BaseViewModel
{
    public AccountViewModel()
    {
        SignOutCommand = new AsyncRelayCommand(OnSignOut);
    }

    public ICommand SignOutCommand { get; }

    private async Task OnSignOut()
    {
        await AuthService.SignOut("0");
    }
}
