using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Stone(IList<IList<Element>> parentList) : UnmovableElement(parentList)
{
    public override Color InsideColor { get; } = new Color(100, 100, 100);
    public override Color OutsideColor => new Color(100, 100, 100);
    
    public override void DoPhysics(int x, int y)
    {
    }

    public override Stone Clone()
    {
        return new Stone(ParentCells);
    }
}
