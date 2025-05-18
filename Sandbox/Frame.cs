using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;

namespace Sandbox;

public sealed class Frame : IDisposable
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
        const int ticksPerSecond = 100;
        const int limitInMicroseconds = 1_000_000 / ticksPerSecond;
        
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        action();
        
        stopwatch.Stop();
            
        Thread.Sleep(stopwatch.Elapsed >= TimeSpan.FromMicroseconds(limitInMicroseconds)  ? TimeSpan.Zero : TimeSpan.FromMicroseconds(limitInMicroseconds) - stopwatch.Elapsed);
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

    public void Dispose()
    {
        _game.Dispose();
    }
}
