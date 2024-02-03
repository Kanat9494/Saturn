using Android.Content;
using Android.Media;
using Android.Views;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;

namespace Saturn.Platforms.Android.CustomControls;

public class MauiVideoPlayer : CoordinatorLayout, MediaPlayer.IOnPreparedListener
{
    public MauiVideoPlayer(Context context, Video video) : base(context)
    {
        _context = context;
        _video = video;

        SetBackgroundColor(Color.Black);

        RelativeLayout relativeLayout = new RelativeLayout(_context)
        {
            LayoutParameters = new CoordinatorLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
            {
                Gravity = (int)GravityFlags.Center
            }
        };

        _videoView = new VideoView(context)
        {
            LayoutParameters = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
        };

        relativeLayout.AddView(_videoView);
        AddView(relativeLayout);

        _videoView.Prepared += OnVideoViewPrepared;
    }

    VideoView _videoView;
    MediaController _mediaController;
    bool _isPrepared;
    Context _context;
    Video _video;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _videoView.Prepared -= OnVideoViewPrepared;
            _videoView.Dispose();
            _videoView = null;
            _video = null;
        }

        base.Dispose(disposing);
    }

    void OnVideoViewPrepared(object sender, EventArgs args)
    {
        _isPrepared = true;
        ((IVideoController)_video).Duration = TimeSpan.FromMilliseconds(_videoView.Duration);
    }

    void UpdateStatus()
    {
        VideoStatus status = VideoStatus.NotReady;

        if (_isPrepared)
            status = _videoView.IsPlaying ? VideoStatus.Playing : VideoStatus.Paused;

        ((IVideoController)_video).Status = status;

        TimeSpan timeSpan = TimeSpan.FromMilliseconds(_videoView.CurrentPosition);
        _video.Position = timeSpan;
    }

    public void UpdatePosition()
    {
        if (Math.Abs(_videoView.CurrentPosition - _video.Position.TotalMilliseconds) > 1000)
        {
            _videoView.SeekTo((int)_video.Position.TotalMilliseconds);
        }
    }

    public void UpdateTransportControlsEnabled()
    {
        if (_video.AreTransportControlsEnabled)
        {
            _mediaController = new MediaController(_context);
            _mediaController.SetMediaPlayer(_videoView);
            _videoView.SetMediaController(_mediaController);
        }
        else
        {
            _videoView.SetMediaController(null);
            if (_mediaController != null)
            {
                _mediaController.SetMediaPlayer(null);
                _mediaController = null;
            }
        }
    }

    public void UpdateSource()
    {
        _isPrepared = false;
        bool hasSetSource = false;

        if (_video.Source is UriVideoSource)
        {
            string uri = (_video.Source as UriVideoSource).Uri;
            if (!string.IsNullOrWhiteSpace(uri))
            {
                _videoView.SetVideoURI(Uri.Parse(uri));
                hasSetSource = true;
            }
        }
        else if (_video.Source is ResourceVideoSource)
        {
            string package = Context.PackageName;
            string path = (_video.Source as ResourceVideoSource).Path;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string assetFilePath = "content://" + package + "/" + path;
                _videoView.SetVideoPath(assetFilePath);
                hasSetSource = true;
            }
        }
        else if (_video.Source is FileVideoSource)
        {
            string filename = (_video.Source as FileVideoSource).File;
            if (!string.IsNullOrWhiteSpace(filename))
            {
                _videoView.SetVideoPath(filename);
                hasSetSource = true;
            }
        }

        if (hasSetSource && _video.AutoPlay)
        {
            _videoView.Start();
        }
    }

    public void UpdateIsLooping()
    {
        if (_video.IsLooping)
        {
            _videoView.SetOnPreparedListener(this);
        }
        else
        {
            _videoView.SetOnPreparedListener(null);
        }
    }

    public void OnPrepared(MediaPlayer mp)
    {
        mp.Looping = _video.IsLooping;
    }

    public void PlayRequested(TimeSpan position)
    {
        _video.Play();
        System.Diagnostics.Debug.WriteLine("Video played");
    }

    public void PauseRequested(TimeSpan position)
    {
        _video.Pause();
        System.Diagnostics.Debug.WriteLine($"Video paused at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
    }

    public void StopRequested(TimeSpan position)
    {
        _video.Pause();
        _video.Stop();
        System.Diagnostics.Debug.WriteLine($"Video stopped at {position.Hours:X2}:{position.Minutes:X2}:{position.Seconds:X2}.");
    }
}
