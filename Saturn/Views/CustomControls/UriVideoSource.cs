namespace Saturn.Views.CustomControls;

public class UriVideoSource : VideoSource
{
    public static readonly BindableProperty UriProperty =
        BindableProperty.Create(nameof(Uri), typeof(string), typeof(UriVideoSource));

    public string Uri
    {
        get => (string)GetValue(UriProperty);
        set => SetValue(UriProperty, value);
    }
}
