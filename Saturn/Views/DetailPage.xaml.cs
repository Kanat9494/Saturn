namespace Saturn.Views;

public partial class DetailPage : ContentPage
{
	public DetailPage()
	{
		InitializeComponent();

		BindingContext = new DetailViewModel();
	}
}