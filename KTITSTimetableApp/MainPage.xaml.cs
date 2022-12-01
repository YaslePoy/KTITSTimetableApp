using System.Net.Http.Json;
using System.Text.Json;
using System.Timers;

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
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(50), Timer_Elapsed);
        var text = ReadTextFile("lessonCall.txt").Result;
        Utils.Calls = JsonSerializer.Deserialize<List<TimeSpan>>(text);
        text = ReadTextFile("122.json").Result;
        Utils.TT = JsonSerializer.Deserialize<List<Lesson[]>>(text);
    }

    private bool Timer_Elapsed()
    {
        Dispatcher.Dispatch(() =>
        {
            var now = DateTime.Now.TimeOfDay;
            NowTimeLb.Text = now.ToString().Substring(0, 10);
            NextTimeLb.Text = Utils.CurrentNextTime.ToString();
            LastTimeLb.Text = Utils.LastTime.ToString();
            var sub = Utils.TrueTimeSub(Utils.CurrentNextTime, now);
            if (sub.TotalMilliseconds > 0)
            {
                EndTimeLb.Text = sub.ToString().Substring(0, 10);
                var lnDelta = Utils.TrueTimeSub(Utils.CurrentNextTime, Utils.LastTime);
                TotalProgressView.Progress = (lnDelta.TotalMilliseconds - sub.TotalMilliseconds) / lnDelta.TotalMilliseconds;
                if (sub.TotalMinutes < 1)
                    HotMode();
                else
                    ColdMode();
                TotalProgressProcent.Text = (TotalProgressView.Progress * 100).ToString().Substring(0, 5) + " %";
            }
            else
            {
                ColdMode();
            }
        });
        return true;
    }
    public void HotMode()
    {
        LastMinSL.IsVisible = true;
        TotalProgressView.ProgressColor = Color.FromRgb(255, 255, 0);
        if(DateTime.Now.TimeOfDay.Seconds < 60)
        LastMinView.Progress = (DateTime.Now.TimeOfDay.Milliseconds + DateTime.Now.TimeOfDay.Seconds * 1000) / 60000d;

    }
    public void ColdMode()
    {
        LastMinSL.IsVisible = false;
        TotalProgressView.ProgressColor = Color.FromRgb(255, 255, 255);
    }

}

