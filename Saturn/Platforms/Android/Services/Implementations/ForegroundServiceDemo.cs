using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.App;

namespace Saturn.Platforms.Android.Services.Implementations;

[Service]
public class ForegroundServiceDemo : Service
{
    private string NOTIFICATION_CHANNEL_ID = "1000";
    private int NOTIFICATION_ID = 1;
    private string NOTIFICATION_CHANNEL_NAME = "notification";

    private async void StartForegroundService()
    {
        var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            CreateNotificationChannel(notificationManager);

        var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
        notification.SetAutoCancel(false);
        notification.SetOngoing(true);
        notification.SetSmallIcon(Resource.Mipmap.appicon);
        notification.SetContentTitle("ForegroundService");
        notification.SetContentText("Foreground Service is running");

        
        StartForeground(NOTIFICATION_ID, notification.Build());

        for (int i = 0; i < 50; i++)
        {
            System.Diagnostics.Debug.WriteLine("Foreground service is running");
            Toast.MakeText(this, "Foreground service in running", ToastLength.Short).Show();
            await Task.Delay(3000);
        }
    }

    private void CreateNotificationChannel(NotificationManager notificationManager)
    {
        var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
        notificationManager.CreateNotificationChannel(channel);
    }

    public override IBinder? OnBind(Intent? intent)
    {
        return null;
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        StartForegroundService();
        return StartCommandResult.NotSticky;
    }
}
