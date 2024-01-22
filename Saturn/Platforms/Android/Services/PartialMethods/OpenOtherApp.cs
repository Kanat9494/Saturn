using Android.Content;
using Android.Content.PM;

namespace Saturn.Services.PartialMethods;

public partial class OpenOtherApp
{
    public partial void LaunchApp(string packageName)
    {
        PackageManager? pm = Platform.AppContext.PackageManager;

        Intent? intent = pm?.GetLaunchIntentForPackage(packageName);

        if (intent != null)
        {
            intent.SetFlags(ActivityFlags.NewTask);
            Platform.AppContext.StartActivity(intent);
        }
    }
}
