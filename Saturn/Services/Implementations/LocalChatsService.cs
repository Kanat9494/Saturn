namespace Saturn.Services.Implementations;

public class LocalChatsService
{
    public LocalChatsService()
    {

    }

    SQLiteAsyncConnection Database;

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(LocalDBConstants.DatabasePath, LocalDBConstants.FLAGS);
        var result = await Database.CreateTableAsync<ChatRoom>();
    }

    public async Task<List<ChatRoom>> GetItemsAsync()
    {
        await Init();
        return await Database.Table<ChatRoom>().ToListAsync();
    }

    public async Task<ChatRoom> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<ChatRoom>().FirstOrDefaultAsync(c => c.ChatId == id);
    }

    public async Task<int> SaveItemAsync(ChatRoom chat)
    {
        await Init();
        if (chat.ChatId != 0)
            return await Database.UpdateAsync(chat);
        else
            return await Database.InsertAsync(chat);
    }

    public async Task<int> DeleteItemAsync(ChatRoom chat)
    {
        await Init();
        return await Database.DeleteAsync(chat);
    }

    public async Task<int> HasUserChat(int userId)
    {
        await Init();
        var existingChat = await Database.Table<ChatRoom>().FirstOrDefaultAsync(c => c.SenderId == userId);

        if (existingChat != null)
            return existingChat.ChatId;

        return 0;
    }
}
