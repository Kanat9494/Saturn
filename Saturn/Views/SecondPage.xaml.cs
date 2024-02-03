namespace Saturn.Views;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();

		BindingContext = new SecondViewModel();
	}
}