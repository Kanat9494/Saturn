using Android.App;
using Android.App.Job;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using Javax.Annotation;
using Kotlin;

namespace Saturn.Platforms.Android.Services.Implementations;

[Service(Exported = true, Name = "com.mycompany.myapp.DeepJobService")]
public class DeepJobService : JobService
{
    const string SAVED_INT_KEY = "int_key";
    const string CHANNEL_ID = "local_notification";
    const string NOTIFICATION_CHANNEL_NAME = "test_notification";
    JobParameters parameters;
    private int NOTIFICATION_ID = 2;
    CounterTask task;
    string TAG = typeof(DeepJobService).Name;
    Task jobTask;
    public override bool OnStartJob(JobParameters? @params)
    {
        this.parameters = @params!;
        //var start = GetValue();

        //task = new CounterTask(this, start);
        //task.Execute(Unit);
        var notificationManager = GetSystemService(NotificationService) as NotificationManager;
        

        jobTask = new Task(async () =>
        {
            for (int i = 0; i < 100; i++)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    CreateNotificationChannel(notificationManager);
                }

                var notification = new NotificationCompat.Builder(this, CHANNEL_ID);
                notification.SetAutoCancel(false);
                notification.SetOngoing(true);
                notification.SetSmallIcon(Resource.Mipmap.appicon);
                notification.SetContentTitle("ForegroundService");
                notification.SetContentText($"Прошло {i * 5} секунд");



                ///notificationManager.Notify(NOTIFICATION_ID, notification.Build());
                StartForeground(NOTIFICATION_ID, notification.Build());
                System.Diagnostics.Debug.WriteLine(i);

                await Task.Delay(5000);
            }


        });
        jobTask.Start();

        return true;
    }

    public override bool OnStopJob(JobParameters? @params)
    {
        return true;
    }

    private void CreateNotificationChannel(NotificationManager notificationManager)
    {
        var channel = new NotificationChannel(CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
        notificationManager.CreateNotificationChannel(channel);
    }
}

public class CounterTask : AsyncTask<Unit, int, Unit>
{
    public CounterTask(DeepJobService @params, int startInt)
    {

    }

    protected override Unit RunInBackground(params Unit[] @params)
    {
        throw new NotImplementedException();
    }
}
