namespace Saturn.Views.Chat;

public partial class ChatPage : ContentPage
{
	public ChatPage(LocalMessagesService messagesService, LocalChatsService chatsService)
	{
		InitializeComponent();

		BindingContext = _viewModel = new ChatViewModel(messagesService, chatsService);

        //contentCV.ScrollTo(_viewModel.Messages.Last());
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

	void ClearNavigationStack()
	{
        var stack = Shell.Current.Navigation.NavigationStack.ToArray();
        for (int i = stack.Length - 1; i > 0; i--)
        {
            Shell.Current.Navigation.RemovePage(stack[i]);
        }
    }
}