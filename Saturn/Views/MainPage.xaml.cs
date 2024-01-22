namespace Saturn.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

        GenerateUIAsync();

        this.BindingContext = _viewModel = new MainViewModel();
	}

	MainViewModel _viewModel;

	private void GenerateUIAsync()
	{
        Dispatcher.Dispatch(async () =>
		{
			_viewModel.IsBusy = true;

            #region skeleton
            contentGrid.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Border
                    {
                        Stroke = Colors.Transparent,
                        Background = Color.FromArgb("#E1E1E1"),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,


                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(6, 6, 6, 6)
                        },
                        HeightRequest = 30
                    }.Margins(10, 0, 10, 0),

                    new Border
                    {
                        Stroke = Colors.Transparent,
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(6, 6, 6, 6)
                        },
                        Background = Color.FromArgb("#E1E1E1"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    }.Height(30).Width(30).Margins(0, 0, 10, 0),

                }
            }.Bind(StackLayout.IsVisibleProperty, static (MainViewModel vm) => vm.IsBusy), 0, 0);

            contentGrid.Add(new StackLayout
            {
                Children =
                {
                    ListViewSkeleton(),
                    ListViewSkeleton(),
                    ListViewSkeleton(),
                }
            }.Margins(10, 0, 10, 0).Bind(StackLayout.IsVisibleProperty, static (MainViewModel vm) => vm.IsBusy), 0, 2);
            //ListViewSkeleton();
            //ListViewSkeleton();
            //ListViewSkeleton();
            //ListViewSkeleton();


            #endregion

            //await _viewModel.Generate
            await Task.Delay(3000);



            #region content

            _viewModel.IsBusy = false;

            contentGrid.Add(new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center
                    }.Text("Saturn").Font(size: 24, bold: true, italic: true, family: "RegularFont"),

                    new Image
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Source = "bell_icon.png",
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    }.Height(25).Width(25)
                }
            }.Margins(10, 10, 10, 0).Bind(StackLayout.IsVisibleProperty, static (MainViewModel vm) => !vm.IsBusy), 0, 0);

            contentGrid.Add(new Border
            {
                Stroke = Colors.Transparent,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(6, 6, 6, 6)
                },
                Background = Color.FromArgb("#E1E1E1"),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = new BorderlessEntry
                {
                    Placeholder = "Нажмите для поиска"
                }.Margins(15, 0, 0, 0)
            }.Margins(10, 0, 10, 0), 0, 1);

            var button = new Button
            {
                Text = "Test",
            };
            button.Clicked += (sender, e) => _viewModel.LaunchApp();
            contentGrid.Add(new StackLayout
            {
                Children = 
                {
                    button,
                    //new Label
                    //{
                    //    Text = AppInfo.Current.VersionString
                    //}
                }
            }, 0, 2);

        #endregion

		});
	}

	private StackLayout ListViewSkeleton()
	{
        return new StackLayout
        {
            Orientation = StackOrientation.Horizontal,

            Children =
            {
                new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(10, 10, 10, 10),
                    },
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Background = Color.FromArgb("#C8C8C8")
                }.Width(130).Margins(0, 0, 10, 0).Height(160),

                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                    }

                }.Margins(0, 0, 0, 0)
            }
        }.Margins(0, 0, 10, 8);
    }
}