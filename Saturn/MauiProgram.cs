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
                })
                .ConfigureLifecycleEvents(lifecycle =>
                {
#if ANDROID
                    lifecycle.AddAndroid(android => {
						android.OnCreate((activity, bundle) =>
						{
							var action = activity.Intent?.Action;
							var data = activity.Intent?.Data?.ToString();

							if (action == Android.Content.Intent.ActionView && data is not null)
							{
								activity.Finish();
								System.Threading.Tasks.Task.Run(() => HandleAppLink(data));
							}
						});
					});
#endif
                });
                

            builder.Services.AddSingleton<LocalMessagesService>();
            builder.Services.AddSingleton<LocalChatsService>();
            builder.Services.AddTransient<ChatsPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddSingleton<ClientWSManager>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        static void HandleAppLink(string url)
        {
            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                App.Current?.SendOnAppLinkRequestReceived(uri);
            }
        }
    }
}
