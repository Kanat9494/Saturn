namespace Saturn.Views.CustomControls;

public class Video : View, IVideoController
{
    public Video()
    {
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }
    public static readonly BindableProperty AreTransportControlsEnabledProperty =
        BindableProperty.Create(nameof(AreTransportControlsEnabled), typeof(bool), typeof(Video), true);
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(Video), null);
    public static readonly BindableProperty AutoPlayProperty =
        BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(Video), true);
    public static readonly BindableProperty IsLoopingProperty =
        BindableProperty.Create(nameof(IsLooping), typeof(bool), typeof(Video), false);
    public bool AreTransportControlsEnabled
    {
        get => (bool)GetValue(AreTransportControlsEnabledProperty);
        set => SetValue(AreTransportControlsEnabledProperty, value);
    }

    [TypeConverter(typeof(VideoSourceConverter))]
    public VideoSource Source
    {
        get => (VideoSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public bool AutoPlay
    {
        get => (bool)GetValue(AutoPlayProperty);
        set => SetValue(AutoPlayProperty, value);
    }

    public bool IsLooping
    {
        get => (bool)GetValue(IsLoopingProperty);
        set => SetValue(IsLoopingProperty, value);
    }

    public event EventHandler<VideoPositionEventArgs> PlayRequested;
    public event EventHandler<VideoPositionEventArgs> PauseRequested;
    public event EventHandler<VideoPositionEventArgs> StopRequested;

    public void Play()
    {
        VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
        PlayRequested?.Invoke(this, args);
        Handler?.Invoke(nameof(Video.PlayRequested), args);
    }

    public void Pause()
    {
        VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
        PauseRequested?.Invoke(this, args);
        Handler?.Invoke(nameof(Video.PauseRequested), args);
    }

    public void Stop()
    {
        VideoPositionEventArgs args = new VideoPositionEventArgs(Position);
        StopRequested?.Invoke(this, args);
        Handler?.Invoke(nameof(Video.StopRequested), args);
    }

    private static readonly BindablePropertyKey StatusPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Status), typeof(VideoStatus), typeof(Video), VideoStatus.NotReady);

    public static readonly BindableProperty StatusProperty = StatusPropertyKey.BindableProperty;
    public VideoStatus Status
    {
        get => (VideoStatus)GetValue(StatusProperty);
    }

    VideoStatus IVideoController.Status
    {
        get => Status;
        set => SetValue(StatusPropertyKey, value);
    }

    public event EventHandler UpdateStatus;
    IDispatcherTimer _timer;

    ~Video() => _timer.Tick -= OnTimerTick;

    void OnTimerTick(object sender, EventArgs e)
    {
        UpdateStatus?.Invoke(this, EventArgs.Empty);
        Handler?.Invoke(nameof(Video.UpdateStatus));
    }

    private static readonly BindablePropertyKey DurationPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Duration), typeof(TimeSpan), typeof(Video), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((Video)bindable).SetTimeToEnd());

    public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

    public TimeSpan Duration
    {
        get { return (TimeSpan)GetValue(DurationProperty); }
    }

    TimeSpan IVideoController.Duration
    {
        get { return Duration; }
        set { SetValue(DurationPropertyKey, value); }
    }

    public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(TimeSpan), typeof(Video), new TimeSpan(),
                propertyChanged: (bindable, oldValue, newValue) => ((Video)bindable).SetTimeToEnd());

    public TimeSpan Position
    {
        get { return (TimeSpan)GetValue(PositionProperty); }
        set { SetValue(PositionProperty, value); }
    }

    private static readonly BindablePropertyKey TimeToEndPropertyKey =
            BindableProperty.CreateReadOnly(nameof(TimeToEnd), typeof(TimeSpan), typeof(Video), new TimeSpan());

    public static readonly BindableProperty TimeToEndProperty = TimeToEndPropertyKey.BindableProperty;

    public TimeSpan TimeToEnd
    {
        get { return (TimeSpan)GetValue(TimeToEndProperty); }
        private set { SetValue(TimeToEndPropertyKey, value); }
    }

    void SetTimeToEnd()
    {
        TimeToEnd = Duration - Position;
    }
}
