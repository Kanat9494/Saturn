namespace Saturn.Views;

public partial class CustomBottomBarPage : ContentPage
{
	public CustomBottomBarPage()
	{
		InitializeComponent();

		BindingContext = new CustomBottomBarViewModel();
	}

	private void ChangeMainPage(object sender, EventArgs e)
	{
		
	}

	private void ToSecondPage(object sender, EventArgs e)
	{
		MainPage = new SecondPage();
	}
}