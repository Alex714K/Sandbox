using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Stone(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : UnmovableElement(parentCells, parentDrawableCells)
{
    protected override void CalculateSelfPhysics(int x, int y)
    {
    }

    protected override void DrawPixel(int x, int y, Color color)
    {
        ParentDrawableCells[y][x] = color;
    }

    protected override Color InsideColor { get; } = new Color(100, 100, 100);
    public override Color OutsideColor => new Color(100, 100, 100);

    public override Stone Clone()
    {
        return new Stone(ParentCells, ParentDrawableCells);
    }
}
