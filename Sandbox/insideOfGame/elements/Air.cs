using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Air(IList<IList<Element>> parentCells) : Element(parentCells)
{
    public override void DoPhysics(int x, int y)
    {
    }

    public override Color InsideColor => Color.White;

    public override Air Clone()
    {
        return new Air(ParentCells);
    }
}
