namespace Saturn.Views.Chat;

public partial class ChatPage : ContentPage
{
	public ChatPage(LocalMessagesService messagesService, LocalChatsService chatsService)
	{
		InitializeComponent();

		BindingContext = _viewModel = new ChatViewModel(messagesService, chatsService);
	}

    ChatViewModel _viewModel;

    protected override void OnAppearing()
    {
        base.OnAppearing();

		_viewModel.OnApearing();
    }

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		_viewModel.OnDisappearing();
	}
}