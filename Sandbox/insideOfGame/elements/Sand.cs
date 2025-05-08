using System.Security.Cryptography;
using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Sand(IList<IList<Element>> parentCells) : Element(parentCells)
{
    #region Color
    private static readonly IList<Color> VariantsOfColors = new List<Color>
    {
        new Color(255, 255, 0),
        new Color(210, 210, 0),
        new Color(190, 190, 0),
        new Color(170, 170, 0)
    }.AsReadOnly();

    public override Color InsideColor { get; } =
        VariantsOfColors[RandomNumberGenerator.GetInt32(0, VariantsOfColors.Count)];
    public override Color OutsideColor => VariantsOfColors[1];
    #endregion

    #region Physics
    public override void DoPhysics(int x, int y)
    {
        if (y + 1 >= Grid.SizeY)
        {
            return;
        }
        
        Grid.CheckAndTellAboutUpperElement(x, y);
    
        if (!TryMoveSandDown(x, y) && !IsAirUnder)
        {
            MoveSandLeftOrRight(x, y);
        }
    }

    private bool TryMoveSandDown(int x, int y)
    {
        if (ParentCells[y + 1][x] is not Air)
        {
            return false;
        }

        Element element = ParentCells[y + 1][x];
        ParentCells[y + 1][x] = this;
        ParentCells[y][x] = element;

        return true;
    }

    private void MoveSandLeftOrRight(int x, int y)
    {
        int randomNumber = RandomNumberGenerator.GetInt32(0, 2);
        
        if (randomNumber == 0)
        {
            if (!TryMoveSandLeft(x, y))
            {
                TryMoveSandRight(x, y);
            }
        }
        else
        {
            if (!TryMoveSandRight(x, y))
            {
                TryMoveSandLeft(x, y);
            }
        }
    }

    private bool TryMoveSandLeft(int x, int y)
    {
        if (x - 1 < 0)
        {
            return false;
        }

        if (ParentCells[y + 1][x - 1] is not Air)
        {
            return false;
        }

        Element cellDownLeft = ParentCells[y + 1][x - 1];
        ParentCells[y + 1][x - 1] = this;
        ParentCells[y][x] = cellDownLeft;

        return true;
    }

    private bool TryMoveSandRight(int x, int y)
    {
        if (x + 1 >= Grid.SizeX)
        {
            return false;
        }

        if (ParentCells[y + 1][x + 1] is not Air)
        {
            return false;
        }

        Element cellDownRight = ParentCells[y + 1][x + 1];
        ParentCells[y + 1][x + 1] = this;
        ParentCells[y][x] = cellDownRight;

        return true;
    }
    #endregion

    public override Sand Clone()
    {
        return new Sand(ParentCells);
    }
}
