using Sandbox.insideOfGame;
using SFML.Graphics;

namespace Sandbox;

public class Game : Drawable, Nextable
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
}