namespace Saturn.Views.Chat;

public partial class ChatPage : ContentPage
{
	public ChatPage(LocalMessagesService messagesService)
	{
		InitializeComponent();

		BindingContext = new ChatViewModel(messagesService);	
	}
}