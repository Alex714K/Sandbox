using System.Security.Cryptography;
using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;

namespace Sandbox.insideOfGame.elements;

public class Sand(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : QuickElement(parentCells, parentDrawableCells)
{
    #region Color
    private static readonly IList<Color> VariantsOfColors = new List<Color>
    {
        new Color(255, 255, 0),
        new Color(210, 210, 0),
        new Color(190, 190, 0),
        new Color(170, 170, 0)
    }.AsReadOnly();

    protected override Color InsideColor { get; } =
        VariantsOfColors[RandomNumberGenerator.GetInt32(0, VariantsOfColors.Count)];
    public override Color OutsideColor => VariantsOfColors[1];
    #endregion

    protected override void DrawPixel(int x, int y, Color color)
    {
        ParentDrawableCells[y][x] = color;
    }

    public override Sand Clone()
    {
        return new Sand(ParentCells, ParentDrawableCells);
    }
}
