using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Air(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : Element(parentCells, parentDrawableCells)
{
    protected override void CalculateSelfPhysics(int x, int y)
    {
    }

    protected override void DrawPixel(int x, int y, Color color)
    {
        ParentDrawableCells[y][x] = color;
    }

    protected override Color InsideColor => Color.White;

    public override Air Clone()
    {
        return new Air(ParentCells, ParentDrawableCells);
    }
}
