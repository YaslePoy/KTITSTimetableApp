using Microsoft.Maui.Controls;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

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
        for (int i = 0; i < Utils.Today.Length * 2; i++)
        {
            LessonsViewer.Add(new Label() { Text = Utils.Calls[i].ToString().Substring(0, 5) });
            Grid grid = new Grid();
            BoxView bv = new BoxView() { Color = Brush.Blue.Color, CornerRadius = new CornerRadius(10) };
            Grid.SetRowSpan(bv, 2);
            grid.Add(bv);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition(0));
            Grid hiddble = new Grid() { IsVisible = false};
            hiddble.RowDefinitions.Add(new RowDefinition());
            hiddble.RowDefinitions.Add(new RowDefinition());
            Grid localTimeGrid = new Grid();
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localTimeGrid.Add(new Label() { Text = "До конца", Margin = new Thickness(10, 0), Background = Brush.Red.Color });
            Label diff = new Label() { Text = "11:11:11.1"};
            Grid.SetColumn(diff, 1);
            localTimeGrid.Add(diff);
            Grid localProgressGrid = new Grid();
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(60));
            ProgressBar pbTotal = new ProgressBar() { Margin = new Thickness(10, 0) };
            Label procentLB = new Label() { FontSize = 15, Text = "12.34%" };
            ProgressBar pbMinute = new ProgressBar() { ScaleY = 3, Margin = new Thickness(10, 0), ProgressColor = Brush.Orange.Color };
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
                Label lb = new Label() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, FontSize = 25, Padding = 3 };
                lb.Text = $"Перемена {(Utils.Calls[i + 1] - Utils.Calls[i]).Minutes} минут";
                grid.Add(lb);
            }
            else
            {
                Lesson lesson = Utils.Today[i / 2];
                if (lesson == null)
                {
                    Label lb = new Label() { Text = "Нет пары", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, FontSize = 25, Padding = 3 };
                    grid.Add(lb);
                }
                else
                {
                    VerticalStackLayout st = new VerticalStackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 5 };
                    Label lbL = new Label() { Text = lesson.LessonName, Margin = new Thickness(17, 0), FontSize = 25 };
                    Label lbN = new Label() { Text = lesson.ClassNo.ToString(), Margin = new Thickness(17, 0), FontSize = 25 };
                    Label lbT = new Label() { Text = lesson.TeacherName, Margin = new Thickness(17, 0), FontSize = 25 };
                    st.Add(lbL);
                    st.Add(lbN);
                    st.Add(lbT);
                    grid.Add(st);
                }

            }
            LessonsViewer.Add(grid);
        }
    }
    private bool Timer_Elapsed()
    {
        Dispatcher.Dispatch(() =>
        {
            var now = DateTime.Now.TimeOfDay;
            NowTimeLb.Text = now.ToString().Substring(0, 10);
        });
        return true;
    }
    public void HotMode()
    {

    }
    public void ColdMode()
    {

    }

}

