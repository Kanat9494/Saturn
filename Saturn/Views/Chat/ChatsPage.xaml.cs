namespace Saturn.Views.Chat;

public partial class ChatsPage : ContentPage
{
	public ChatsPage(ChatsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = _viewModel = viewModel;
	}

	ChatsViewModel _viewModel;

    protected override void OnAppearing()
    {
        base.OnAppearing();

		_viewModel.OnAppearing();
    }

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		_viewModel.OnDisappearing();
	}
	
}