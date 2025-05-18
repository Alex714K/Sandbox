using Sandbox.insideOfGame;
using SFML.Graphics;
using SFML.System;

namespace Sandbox;

public sealed class Game : Drawable, INextable, IDisposable
{
    private readonly Gui _gui = new Gui();
    private readonly Grid _grid = new Grid();
    
    private readonly RectangleShape _backgroundShape = new RectangleShape
    {
        Size = new Vector2f(Frame.Window.Size.X, Frame.Window.Size.Y),
        FillColor = Color.Black,
    };

    public void Draw(RenderTarget target, RenderStates states)
    {
        target.Draw(_backgroundShape);
        target.Draw(_gui);
        target.Draw(_grid);
    }

    public void Next()
    {
        _grid.Next();
    }

    public void Dispose()
    {
        _grid.Dispose();
    }
}
