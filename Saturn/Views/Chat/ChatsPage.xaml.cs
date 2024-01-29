namespace Saturn.Views.Chat;

public partial class ChatsPage : ContentPage
{
	public ChatsPage(LocalChatsService chatsService)
	{
		InitializeComponent();

		BindingContext = new ChatsViewModel(chatsService);
	}
}