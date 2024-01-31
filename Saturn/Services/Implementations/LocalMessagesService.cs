namespace Saturn.Services.Implementations;

public class LocalMessagesService
{
    public LocalMessagesService()
    {

    }

    SQLiteAsyncConnection Database;

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(LocalDBConstants.DatabasePath, LocalDBConstants.FLAGS);
        var result = await Database.CreateTableAsync<Message>();
    }

    public async Task<List<Message>> GetItemsAsync()
    {
        await Init();
        return await Database.Table<Message>().ToListAsync();
    }

    public async Task<List<Message>> GetChatMessagesAsync(int chatId)
    {
        await Init();
        return await Database.Table<Message>().Where(m => m.ChatId == chatId).ToListAsync();
    }

    public async Task<int> SaveItemAsync(Message message)
    {
        await Init();
        if (message.MessageId != 0)
            return await Database.UpdateAsync(message);
        else
            return await Database.InsertAsync(message);
    }

    public async Task<int> DeleteItemAsync(Message message)
    {
        await Init();
        return await Database.DeleteAsync(message);
    }

    public async Task<List<Message>> GetChatMessages(int chatId)
    {
        await Init();
        return await Database.Table<Message>().Where(m => m.ChatId == chatId).ToListAsync();
    }

    public async Task<int> GetReceiverId(int chatId)
    {
        await Init();
        var chat = await Database.Table<Message>().FirstOrDefaultAsync(m => m.ChatId == chatId);
        return chat?.SenderId ?? 0;
    }
}
