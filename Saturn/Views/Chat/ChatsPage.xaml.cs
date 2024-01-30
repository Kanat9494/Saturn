namespace Saturn.Views.Chat;

public partial class ChatsPage : ContentPage
{
	public ChatsPage(LocalChatsService chatsService, LocalMessagesService messagesService)
	{
		InitializeComponent();

		BindingContext = new ChatsViewModel(chatsService, messagesService);
	}
}