#if ANDROID
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
#endif


namespace Saturn
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if __ANDROID__
                //handler.PlatformView.Background = null;
                //handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
#elif __IOS__
			    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
			    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            if ((uri.Host.ToLower() == "letoinc.saturn" || uri.Host.ToLower() == "letoinc.saturn.kg") && uri.Segments != null && uri.Segments.Length == 3)
            {
                string action = uri.Segments.ElementAt(1).Replace("/", "");
                bool isActionParamasValid = long.TryParse(uri.Segments.ElementAt(2), out long blogId);
                if (action.ToLower() == "blog-post-details" && isActionParamasValid)
                {
                    if (blogId > 0)
                    {
                        Shell.Current.GoToAsync("DetailsPageFromDeeplink");
                    }
                    else
                    {
                        Shell.Current.GoToAsync("MainPage");
                    }
                }
            }
        }
    }
}
