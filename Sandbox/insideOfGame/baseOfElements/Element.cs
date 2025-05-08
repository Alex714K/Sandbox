using SFML.Graphics;

namespace Sandbox.insideOfGame.baseOfElements;

public abstract class Element(IList<IList<Element>> parentCells) : IClonable<Element>
{
    public virtual bool Movable => true;

    protected IList<IList<Element>> ParentCells { get; } = parentCells;
    public abstract void DoPhysics(int x, int y);

    public abstract Color InsideColor { get; }
    public virtual Color OutsideColor => Color.White;

    private bool _isAirUnder;

    public bool IsAirUnder
    {
        get
        {
            bool condition = _isAirUnder;
            _isAirUnder = false;
            return condition;
        }
        set => _isAirUnder = value;
    }

    public abstract Element Clone();
}
