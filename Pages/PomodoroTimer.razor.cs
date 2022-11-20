using Microsoft.AspNetCore.Components;

namespace BlazorCourse.Pages;

public class PomodoroTimerBase : ComponentBase, IDisposable
{
    private const int PomodoroSeconds = 25 * 60;
    private int SecondsLeft { get; set; } = PomodoroSeconds;
    public string SecondsLeftString => TimeSpan.FromSeconds(SecondsLeft).ToString(@"mm\:ss");
    private PeriodicTimer? Timer { get; set; } = null;
    private bool IsRunning => Timer != null;
    public bool IsPaused { get; set; } = false;

    public async Task StartTimer()
    {
        if (IsRunning)
        {
            return;
        }

        IsPaused = false;
        Timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        while (SecondsLeft > 0)
        {
            var ranTimer = await Timer.WaitForNextTickAsync();
            if (ranTimer == false)
            {
                Timer = null;
                break;
            }

            SecondsLeft--;
            await InvokeAsync(StateHasChanged);
        }
    }

    public void StopTimer()
    {
        IsPaused = false;
        Timer?.Dispose();
        SecondsLeft = PomodoroSeconds;
    }

    public void PauseTimer()
    {
        IsPaused = true;
        Timer?.Dispose();
    }

    public void Dispose()
    {
        Timer?.Dispose();
    }
}