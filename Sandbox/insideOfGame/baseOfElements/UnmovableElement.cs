namespace Sandbox.insideOfGame.baseOfElements;

public abstract class UnmovableElement(IList<IList<Element>> parentCells) : Element(parentCells)
{
    public override bool Movable => false;
}