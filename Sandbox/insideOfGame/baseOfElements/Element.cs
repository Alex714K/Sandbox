using SFML.Graphics;

namespace Sandbox.insideOfGame.baseOfElements;

public abstract class Element(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : IClonable<Element>
{
    protected IList<IList<Element>> ParentCells { get; } = parentCells;
    protected IList<IList<Color>> ParentDrawableCells { get; } = parentDrawableCells;
    protected abstract void CalculateSelfPhysics(int x, int y);
    protected abstract void DrawPixel(int x, int y, Color color);

    public void CalculateSelfLogic(int x, int y)
    {
        DrawPixel(x, y, InsideColor);
        CalculateSelfPhysics(x, y);
    }

    protected abstract Color InsideColor { get; }
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
