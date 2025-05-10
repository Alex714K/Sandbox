using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace Sandbox;

public class Frame
{
    public static readonly RenderWindow Window = new RenderWindow(
        new VideoMode(1920, 1200), "Sandbox");
    
    private readonly Game _game = new Game();
    
    public Frame()
    {
        Window.Closed += OnClose;
    }

    public void Start()
    {
        Window.SetActive(false);
        
        var cycleDraw = new Thread(CycleDraw);
        
        cycleDraw.Start();

        while (Window.IsOpen)
        {
            DoWithLimit(DoNext);
        }
    }

    private static void DoWithLimit(Action action)
    {
        const int ticksPerSecond = 125;
        const int limitInMilliseconds = 1000 / ticksPerSecond;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        action();
        
        stopwatch.Stop();
            
        Thread.Sleep(stopwatch.Elapsed.Milliseconds >= limitInMilliseconds ? 0 : limitInMilliseconds - stopwatch.Elapsed.Milliseconds);
    }

    private void DoNext()
    {
        Window.DispatchEvents();
            
        _game.Next();
    }

    private void CycleDraw()
    {
        Window.SetActive(true);
        
        while (Window.IsOpen)
        {
            Window.Clear();
            
            Window.Draw(_game);
            
            Window.Display();
        }
    }

    private static void OnClose(object? sender, EventArgs e)
    {
        Window.Close();
    }
}
