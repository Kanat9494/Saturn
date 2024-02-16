using Microsoft.Extensions.Logging;

namespace Saturn
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
                

            builder.Services.AddSingleton<LocalMessagesService>();
            builder.Services.AddSingleton<LocalChatsService>();
            builder.Services.AddTransient<ChatsPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddSingleton<ClientWSManager>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
