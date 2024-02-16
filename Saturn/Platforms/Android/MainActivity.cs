using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Saturn
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(new[] {Android.Content.Intent.ActionView}, 
        DataScheme = "https", 
        DataHost = "letoinc.saturn.kg", 
        DataPathPrefix = "/blog-post-details",
        AutoVerify = true,
        Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable})]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var url = Intent?.DataString;
            Intent? intent = this.Intent;
            var action = intent?.Action;
            if (!string.IsNullOrWhiteSpace(url))
            {
                Microsoft.Maui.Controls.Application.Current.SendOnAppLinkRequestReceived(new Uri(url));

            }
            

            //try
            //{
            //    var jobScheduler = GetSystemService(Android.Content.Context.JobSchedulerService) as JobScheduler;
            //    var componentName = new ComponentName(this, Java.Lang.Class.FromType(typeof(Platforms.Android.Services.Implementations.DeepJobService)));
            //    var jobInfo = new JobInfo.Builder(123, componentName);
            //    var job = jobInfo?.SetRequiresCharging(false)?.SetMinimumLatency(1)?.SetRequiredNetworkType(NetworkType.Any)?.SetOverrideDeadline(3 * 60 * 1000)?.Build();
            //    jobScheduler?.Schedule(job);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine(ex.Message);
            //}
        }
    }
}
