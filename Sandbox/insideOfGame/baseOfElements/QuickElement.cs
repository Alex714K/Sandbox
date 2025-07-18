using System.Security.Cryptography;
using Sandbox.insideOfGame.elements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.baseOfElements;

public abstract class QuickElement(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : Element(parentCells, parentDrawableCells)
{
    #region Logic
    protected override void CalculateSelfPhysics(int x, int y)
    {
        if (y + 1 >= Grid.SizeY)
        {
            return;
        }

        if (!TryMoveSandDown(x, y))
        {
            MoveSandLeftOrRight(x, y);
        }

        Grid.CheckAndTellAboutUpperElement(x, y);
    }

    private bool TryMoveSandDown(int x, int y)
    {
        if (ParentCells[y + 1][x] is UnmovableElement || ParentCells[y + 1][x] is Sand)
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
        if (IsAirUnder)
        {
            return;
        }

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

        if (ParentCells[y + 1][x - 1] is UnmovableElement || ParentCells[y + 1][x - 1] is Sand)
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

        if (ParentCells[y + 1][x + 1] is UnmovableElement || ParentCells[y + 1][x + 1] is Sand)
        {
            return false;
        }

        Element cellDownRight = ParentCells[y + 1][x + 1];
        ParentCells[y + 1][x + 1] = this;
        ParentCells[y][x] = cellDownRight;

        return true;
    }
    #endregion
}
