using Sandbox.insideOfGame;
using SFML.Graphics;

namespace Sandbox;

public sealed class Game : Drawable, INextable, IDisposable
{
    private readonly Grid _grid = new Grid();

    public void Draw(RenderTarget target, RenderStates states)
    {
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
