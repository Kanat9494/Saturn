namespace Saturn.Views;

public partial class DetailsPageFromDeeplink : ContentPage
{
	public DetailsPageFromDeeplink()
	{
		InitializeComponent();


    }

    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
        return false;
    }

    void ClearNavigationStack()
	{
        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
        for (int i = stack.Length - 1; i > 0; i--)
        {
            Shell.Current.Navigation.RemovePage(stack[i]);
        }
    }
}