﻿using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

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
                }).ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(Video), typeof(VideoHandler));
                });
                

            builder.Services.AddSingleton<LocalMessagesService>();
            builder.Services.AddSingleton<LocalChatsService>();
            builder.Services.AddTransient<ChatsPage>();
            builder.Services.AddTransient<ChatPage>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
