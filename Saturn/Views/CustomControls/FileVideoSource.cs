﻿namespace Saturn.Views.CustomControls;

public class FileVideoSource : VideoSource
{
    public static readonly BindableProperty FileProperty =
        BindableProperty.Create(nameof(File), typeof(string), typeof(FileVideoSource));

    public string File
    {
        get => (string)GetValue(FileProperty);
        set => SetValue(FileProperty, value);
    }
}
