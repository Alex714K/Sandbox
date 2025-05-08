using SFML.Graphics;

namespace Sandbox.insideOfGame;

public class Cell
{
    // public IElement? _insideElement = new Air();
    
    private IList<IList<Cell>> _parentCells;
    private int _x;
    private int _y;
    
    public Cell(IList<IList<Cell>> parentCells,  int x, int y)
    {
        _parentCells = parentCells;
        _x = x;
        _y = y;
    }
    
    #region Boolling
    private Color _insideColor = Color.White;

    public Color InsideColor
    {
        get
        {
            Movable = true;
            return _insideColor;
        }
        set
        {
            Movable = _insideColor != Color.White;
            _insideColor = value;
        }
    }
    
    private bool _haveSand = false;
    public bool HaveSand
    {
        get
        {
            Movable = true;
            return _haveSand;
        }
        set
        {
            Movable = !value;
            _haveSand = value;
        }
    }

    public bool Movable { get; private set; } = true;

    private bool _haveSandUnder = false;

    public bool HaveSandUnder
    {
        get
        {
            if (!_haveSandUnder) return false;
            
            _haveSandUnder = false;
            
            return true;
        }
        set => _haveSandUnder = value;
    }
    #endregion
}