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
        int callId = 0;
        foreach (var lesson in Utils.Today)
        {
            LessonsViewer.Add(new Label() { Text = Utils.Calls[callId++].ToString().Substring(0, 5) });
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add( new RowDefinition(45));
            Grid localProgressGrid = new Grid();
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition());
            localProgressGrid.ColumnDefinitions.Add(new ColumnDefinition(60));
            ProgressBar pbTotal = new ProgressBar();
            Label procentLB = new Label() { Text = "12.34%"};
            ProgressBar pbMinute = new ProgressBar() { ScaleY = 3, ProgressColor = Brush.Orange.Color };
            localProgressGrid.Add(pbTotal);
            localProgressGrid.Add(procentLB);
            localProgressGrid.Add(pbMinute);
            Grid.SetColumn(procentLB, 1);
            Grid.SetColumnSpan(pbMinute, 2);
            grid.Add(localProgressGrid);
            Grid.SetRow(localProgressGrid, 1);
            BoxView bv = new BoxView() { HeightRequest = 150, Color = Color.FromRgb(0, 0, 255), CornerRadius = new CornerRadius(10) };
            grid.Add(bv);
            if (lesson is null)
            {
                bv.HeightRequest = 50;
                Label lb = new Label() { Text = "Нет пары", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, FontSize = 25 };
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
            LessonsViewer.Add(grid);
            LessonsViewer.Add(new Label() { Text = Utils.Calls[callId++].ToString().Substring(0, 5) });
            Grid recess = new Grid();
            BoxView recBV = new BoxView() { HeightRequest = 50, Color = Color.FromRgb(0, 0, 255), CornerRadius = new CornerRadius(10) };
            recess.Add(recBV);
            Label theEnd = new Label() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, FontSize = 25 };
            if (Utils.Today.Last() == lesson)
                theEnd.Text = "End of hell";
            else
                theEnd.Text = $"Перемена {(Utils.Calls[callId] - Utils.Calls[callId - 1]).Minutes} минут";
            recess.Add(theEnd);
            LessonsViewer.Add(recess);
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

