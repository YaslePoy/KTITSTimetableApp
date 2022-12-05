using Android.OS;
using System.Diagnostics;
using System.Text.Json;

namespace KTITSTimetableApp;

public partial class MainPage : ContentPage
{
    System.Timers.Timer Timer;
    int count = 0;
    public async Task<string> ReadTextFile(string filePath)
    {
        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(filePath);
        using StreamReader reader = new StreamReader(fileStream);

        return reader.ReadToEndAsync().Result;
    }
    public MainPage()
    {
        InitializeComponent();
        NowTimeLb.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 10);
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), Timer_Elapsed);
        var text = ReadTextFile("lessonCall.txt").Result;
        Utils.Calls = JsonSerializer.Deserialize<List<TimeSpan>>(text);
        text = ReadTextFile("122.json").Result;
        Utils.TT = JsonSerializer.Deserialize<List<Lesson[]>>(text);
        Utils.UpdateTT();
        LoadTT();
    }

    private void LoadTT()
    {
        var allLess = Utils.Calls.Where(i => i < DateTime.Now.TimeOfDay);
        var start = 0;
        if (allLess.Count() > 0)
            start = Utils.Calls.IndexOf(allLess.Last());
        for (int i = start; i < Utils.Today.Length * 2; i++)
        {
            LessonsViewer.Add(new Label() { Text = Utils.Calls[i].ToString().Substring(0, 5) });
            Grid grid = new Grid() { BindingContext = new List<TimeSpan>() { Utils.Calls[i], Utils.Calls[i + 1] } };
            BoxView bv = new BoxView()
            {
                Color = Brush.Blue.Color,
                CornerRadius = new CornerRadius(10)
            };
            Grid.SetRowSpan(bv, 2);
            grid.Add(bv);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition(0));
            Grid hiddble = new Grid() { IsVisible = false };
            hiddble.RowDefinitions.Add(new RowDefinition());
            hiddble.RowDefinitions.Add(new RowDefinition());
            Grid localTimeGrid = new Grid();
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.Add(new Label()
            {
                Text = "До конца",
                Margin = new Thickness(10, 0),
            });
            Label diff = new Label() { Text = "11:11:11.1" };
            Grid.SetColumn(diff, 1);
            localTimeGrid.Add(diff);
            Grid localProgressGrid = new Grid();
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(60));
            ProgressBar pbTotal = new ProgressBar() { Margin = new Thickness(10, 0) };
            Label procentLB = new Label() { FontSize = 15, Text = "12.34%" };
            ProgressBar pbMinute = new ProgressBar()
            {
                ScaleY = 3,
                Margin = new Thickness(10, 0),
                ProgressColor = Brush.Orange.Color,
                IsVisible = false
            };
            localProgressGrid.Add(pbTotal);
            localProgressGrid.Add(procentLB);
            localProgressGrid.Add(pbMinute);
            Grid.SetColumn(procentLB, 1);
            Grid.SetColumnSpan(pbMinute, 2);
            hiddble.Add(localProgressGrid);
            hiddble.Add(localTimeGrid);
            Grid.SetRow(localProgressGrid, 1);
            grid.Add(hiddble);
            Grid.SetRow(hiddble, 1);
            if (i % 2 == 1)
            {
                if (i != Utils.Today.Length * 2 - 1)
                {
                    Label lb = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 25,
                        Padding = 3
                    };
                    lb.Text = $"Перемена {(Utils.Calls[i + 1] - Utils.Calls[i]).Minutes} минут";
                    grid.Add(lb);
                }
                else
                {
                    Label lb = new Label()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 25,
                        Padding = 3
                    };
                    lb.Text = $"Домооооой!";
                    grid.Add(lb);
                }

            }
            else
            {
                Lesson lesson = Utils.Today[i / 2];
                if (lesson == null)
                {
                    Label lb = new Label()
                    {
                        Text = "Нет пары",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        FontSize = 25,
                        Padding = 3
                    };
                    grid.Add(lb);
                }
                else
                {
                    VerticalStackLayout st = new VerticalStackLayout()
                    { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 5 };
                    Label lbL = new Label()
                    {
                        Text = lesson.LessonName,
                        Margin = new Thickness(17, 0),
                        FontSize = 25
                    };
                    Label lbN = new Label()
                    {
                        Text = lesson.ClassNo.ToString(),
                        Margin = new Thickness(17, 0),
                        FontSize = 25
                    };
                    Label lbT = new Label()
                    {
                        Text = lesson.TeacherName,
                        Margin = new Thickness(17, 0),
                        FontSize = 25
                    };
                    st.Add(lbL);
                    st.Add(lbN);
                    st.Add(lbT);
                    grid.Add(st);
                }

            }
            LessonsViewer.Add(grid);
        }
        if(LessonsViewer.Children.Count() == 0)
        {
            LessonsViewer.Add(new Label() { Text = Utils.Calls[Utils.Today.Count() * 2 - 1].ToString().Substring(0, 5) });
            Grid grid = new Grid() { BindingContext = new List<TimeSpan>() { Utils.Calls[Utils.Today.Count() * 2 - 1], Utils.Calls[0] } };
            BoxView bv = new BoxView()
            {
                Color = Brush.Blue.Color,
                CornerRadius = new CornerRadius(10)
            };
            Grid.SetRowSpan(bv, 2);
            grid.Add(bv);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition(0));
            Grid hiddble = new Grid() { IsVisible = false };
            hiddble.RowDefinitions.Add(new RowDefinition());
            hiddble.RowDefinitions.Add(new RowDefinition());
            Grid localTimeGrid = new Grid();
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.Add(new Label()
            {
                Text = "До конца",
                Margin = new Thickness(10, 0),
            });
            Label diff = new Label() { Text = "11:11:11.1" };
            Grid.SetColumn(diff, 1);
            localTimeGrid.Add(diff);
            Grid localProgressGrid = new Grid();
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(60));
            ProgressBar pbTotal = new ProgressBar() { Margin = new Thickness(10, 0) };
            Label procentLB = new Label() { FontSize = 15, Text = "12.34%" };
            ProgressBar pbMinute = new ProgressBar()
            {
                ScaleY = 3,
                Margin = new Thickness(10, 0),
                ProgressColor = Brush.Orange.Color,
                IsVisible = false
            };
            localProgressGrid.Add(pbTotal);
            localProgressGrid.Add(procentLB);
            localProgressGrid.Add(pbMinute);
            Grid.SetColumn(procentLB, 1);
            Grid.SetColumnSpan(pbMinute, 2);
            hiddble.Add(localProgressGrid);
            hiddble.Add(localTimeGrid);
            Grid.SetRow(localProgressGrid, 1);
            grid.Add(hiddble);
            Grid.SetRow(hiddble, 1);
            Label lb = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 25,
                Padding = 3
            };
            lb.Text = $"Домооооой!";
            grid.Add(lb);
            LessonsViewer.Add(grid);
        }
        else
        {

        }
        LessonsViewer.Add(new Label() { Text = Utils.Calls[0].ToString().Substring(0, 5) });
    }
    private bool Timer_Elapsed()
    {
        Dispatcher.Dispatch(() =>
        {

            try
            {
                var now = DateTime.Now.TimeOfDay;
                NowTimeLb.Text = now.ToString().Substring(0, 10);
                var currentBlock = LessonsViewer[1] as Grid;
                var bc = currentBlock.BindingContext as List<TimeSpan>;
                var blockCld = currentBlock[1] as Grid;
                blockCld.IsVisible = true;
                currentBlock.RowDefinitions[1].Height = 45;
                var diffTimer = (blockCld[1] as Grid)[1] as Label;
                var timeDiff = Utils.TrueTimeSub(bc[1], now);
                diffTimer.Text = timeDiff.ToString().Substring(0, 10);
                var mainPB = (blockCld[0] as Grid)[0] as ProgressBar;
                var prc = (blockCld[0] as Grid)[1] as Label;
                var minutePB = (blockCld[0] as Grid)[2] as ProgressBar;
                if (timeDiff.TotalMinutes > 1)
                {
                    var lnDelta = Utils.TrueTimeSub(bc[1], bc[0]);
                    mainPB.Progress = (lnDelta.TotalMilliseconds - timeDiff.TotalMilliseconds) / lnDelta.TotalMilliseconds;
                    prc.Text = (mainPB.Progress * 100).ToString().Substring(0, 5) + "%";
                }
                else
                {
                    if (mainPB.IsVisible)
                    {

                        mainPB.IsVisible = false;
                        prc.IsVisible = false;
                        minutePB.IsVisible = true;
                    }
                    mainPB.Progress = (60000 - timeDiff.TotalMilliseconds) / 60000;
                }
            }
            catch(Exception ex)
            {
            }
        });
        return true;
    }
    public Grid NewTimeBlock(TimeSpan start, TimeSpan end, IView content)
    {
        Grid grid = new Grid() { BindingContext = new List<TimeSpan>() { Utils.Calls[Utils.Today.Count() * 2 - 1], Utils.Calls[0] } };
        BoxView bv = new BoxView()
        {
            Color = Brush.Blue.Color,
            CornerRadius = new CornerRadius(10)
        };
        Grid.SetRowSpan(bv, 2);
        grid.Add(bv);
        grid.RowDefinitions.Add(new RowDefinition());
        grid.RowDefinitions.Add(new RowDefinition(0));
        Grid hiddble = new Grid() { IsVisible = false };
        hiddble.RowDefinitions.Add(new RowDefinition());
        hiddble.RowDefinitions.Add(new RowDefinition());
        Grid localTimeGrid = new Grid();
        localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
        localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
        localTimeGrid.Add(new Label()
        {
            Text = "До конца",
            Margin = new Thickness(10, 0),
        });
        Label diff = new Label() { Text = "11:11:11.1" };
        Grid.SetColumn(diff, 1);
        localTimeGrid.Add(diff);
        Grid localProgressGrid = new Grid();
        localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition());
        localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(60));
        ProgressBar pbTotal = new ProgressBar() { Margin = new Thickness(10, 0) };
        Label procentLB = new Label() { FontSize = 15, Text = "12.34%" };
        ProgressBar pbMinute = new ProgressBar()
        {
            ScaleY = 3,
            Margin = new Thickness(10, 0),
            ProgressColor = Brush.Orange.Color,
            IsVisible = false
        };
        localProgressGrid.Add(pbTotal);
        localProgressGrid.Add(procentLB);
        localProgressGrid.Add(pbMinute);
        Grid.SetColumn(procentLB, 1);
        Grid.SetColumnSpan(pbMinute, 2);
        hiddble.Add(localProgressGrid);
        hiddble.Add(localTimeGrid);
        Grid.SetRow(localProgressGrid, 1);
        grid.Add(hiddble);
        Grid.SetRow(hiddble, 1);
        grid.Add(content);
        return grid;
    }
}

