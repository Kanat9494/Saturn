namespace Saturn.Views.Chat;

public partial class ChatsPage : ContentPage
{
	public ChatsPage(LocalChatsService chatsService, LocalMessagesService messagesService)
	{
		InitializeComponent();

		BindingContext = _viewModel = new ChatsViewModel(chatsService, messagesService);
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