using System.Security.Cryptography;
using Sandbox.buttonCore;
using Sandbox.insideOfGame.baseOfElements;
using Sandbox.insideOfGame.elements;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Sandbox.insideOfGame;

public sealed class Grid :
    Drawable,
    Nextable,
    IDisposable
{
    #region Positioning
    private const int PositionY = 100;

    private const int CellSize = 6;
    public static readonly int SizeX = (int)Frame.Window.Size.X / CellSize;
    public static readonly int SizeY = ((int)Frame.Window.Size.Y - PositionY * 2 + PositionY / 2) / CellSize;
    #endregion

    private static readonly Brush LocalBrush = new Brush();

    #region Cells
    private static readonly List<IList<Color>> FrameOfDrawableCells = CreateColorCells();
    
    private static readonly List<IList<Color>> DrawableCells = CreateColorCells();
    private static readonly List<IList<Element>> Cells = CreateCells();

    private static List<IList<Element>> CreateCells()
    {
        var cells = new List<IList<Element>>();
        for (var y = 0; y < SizeY; y++)
        {
            cells.Add([]);
            for (var x = 0; x < SizeX; x++)
            {
                cells[y].Add(new Air(cells, DrawableCells));
            }
        }
        return cells;
    }

    private static List<IList<Color>> CreateColorCells()
    {
        var cells = new List<IList<Color>>();
        for (var y = 0; y < SizeY; y++)
        {
            cells.Add([]);
            for (var x = 0; x < SizeX; x++)
            {
                cells[y].Add(new Color(255, 255, 255));
            }
        }
        return cells;
    }

    private readonly RectangleShape _cellBody = new RectangleShape(new Vector2f(CellSize, CellSize));
    #endregion

    #region Buttons
    private readonly List<Button> _buttons = CreateButtons();

    private static List<Button> CreateButtons()
    {

        var elements = new List<Element>
        {
            new Sand(Cells, DrawableCells),
            new Stone(Cells, DrawableCells),
        };

        const int distanceBetweenButtons = 10;

        const int width = 120;
        const int height = 60;

        return elements.Select((element, i) =>
            ButtonBuilder.Create()
                .SetColor(element.OutsideColor)
                .SetText(element.GetType().Name)
                .SetPosition(new Vector2f(distanceBetweenButtons * (i + 1) + width * i, 20))
                .SetSize(new Vector2f(width, height))
                .Build(() => LocalBrush.ElementForPaint = element)
        ).ToList();
    }
    #endregion

    public void Draw(RenderTarget target, RenderStates states)
    {
        for (int y = SizeY - 1; y >= 0; y--)
        {
            for (var x = 0; x < SizeX; x++)
            {
                Color currentColor = FrameOfDrawableCells[y][x];
                if (currentColor == Color.White)
                {
                    continue;
                }
                _cellBody.Position = new Vector2f(x * CellSize, y * CellSize + PositionY);
                _cellBody.FillColor = currentColor;
                target.Draw(_cellBody);
            }
        }

        _buttons.ForEach(target.Draw);
    }

    public void Next()
    {
        LocalBrush.Next();
        CalculatePhysics();
        
        for (var y = 0; y < SizeY; y++)
        {
            for (var x = 0; x < SizeX; x++)
            {
                FrameOfDrawableCells[y][x] = DrawableCells[y][x];
            }
        }
    }

    private static void CalculatePhysics()
    {
        const int numberFor50PercentChance = 2;

        for (int y = SizeY - 1; y >= 0; y--)
        {
            if (RandomNumberGenerator.GetInt32(0, numberFor50PercentChance) == 0)
            {
                for (var x = 0; x < SizeX; x++)
                {
                    Cells[y][x].CalculateSelfLogic(x, y);
                }
            }
            else
            {
                for (int x = SizeX - 1; x >= 0; x--)
                {
                    Cells[y][x].CalculateSelfLogic(x, y);
                }
            }
        }
    }

    public static void CheckAndTellAboutUpperElement(Vector2i position) => CheckAndTellAboutUpperElement(position.X, position.Y);

    public static void CheckAndTellAboutUpperElement(int x, int y)
    {
        if (y - 1 < 0)
        {
            return;
        }

        if (Cells[y - 1][x] is not Air)
        {
            return;
        }

        const int minimumDistanceToElements = 6;
        const int maximumDistanceToElements = 56;

        for (int i = y - 1; i >= Math.Abs(y - RandomNumberGenerator.GetInt32(minimumDistanceToElements, maximumDistanceToElements)); i--)
        {
            if (Cells[i][x] is UnmovableElement)
            {
                return;
            }

            if (Cells[i][x] is not Sand)
            {
                continue;
            }

            Cells[i][x].IsAirUnder = true;
        }
    }

    public void Dispose()
    {
        _cellBody.Dispose();
        _buttons.ForEach(button => button.Dispose());
    }

    #region Brush
    private sealed class Brush : Nextable,
        IDisposable
    {
        public Element ElementForPaint = new Air(Cells, DrawableCells);

        private static int _radiusOfBrush = 1;
        private static int _radiusMultiply = 1;
        private const int IfMultiply = 2;
        private const int IfNotMultiply = 1;

        static Brush()
        {
            Frame.Window.KeyPressed += OnKeyPress;
            Frame.Window.KeyReleased += OnKeyRelease;
        }

        #region KeyLogic
        private static void OnKeyPress(object? sender, KeyEventArgs e)
        {
            _radiusMultiply = e.Shift ? IfMultiply : IfNotMultiply;
            
            if (e.Code is < Keyboard.Key.Num1 or > Keyboard.Key.Num9)
            {
                return;
            }

            const int speedOfChanging = 2;
            _radiusOfBrush = 1 + speedOfChanging * ((int)e.Code % (int)Keyboard.Key.Num1);
        }

        private static void OnKeyRelease(object? sender, KeyEventArgs e)
        {
            _radiusMultiply = e.Shift ? IfMultiply : IfNotMultiply;
        }
        #endregion

        public void Next()
        {
            CheckMouse();
        }

        #region CheckMouse
        private void CheckMouse()
        {
            if (!Frame.Window.HasFocus())
            {
                return;
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2i position = Mouse.GetPosition(Frame.Window);

                if (!IsInsideOfGridAndWindow(position))
                {
                    return;
                }

                ChangeElementOnMouse((position - new Vector2i(0, PositionY)) / CellSize, true);
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                Vector2i position = Mouse.GetPosition(Frame.Window);

                if (!IsInsideOfGridAndWindow(position))
                {
                    return;
                }

                ChangeElementOnMouse((position - new Vector2i(0, PositionY)) / CellSize, false);
            }
        }

        private void ChangeElementOnMouse(Vector2i position, bool condition)
        {
            for (int y = position.Y - _radiusOfBrush * _radiusMultiply + 1; y < position.Y; y++)
            {
                for (int x = position.X - (_radiusOfBrush * _radiusMultiply - position.Y + y);
                     x < position.X + _radiusOfBrush * _radiusMultiply - position.Y + y + 1;
                     x++)
                {
                    PlaceElementOnCoordinates(x, y, condition);
                }
            }

            for (int x = position.X - _radiusOfBrush * _radiusMultiply + 1; x <= position.X + _radiusOfBrush * _radiusMultiply - 1; x++)
            {
                if (x < 0 || x >= SizeX)
                {
                    continue;
                }

                PlaceElementOnCoordinates(x, position.Y, condition);
            }

            for (int y = position.Y + 1; y < position.Y + _radiusOfBrush * _radiusMultiply; y++)
            {
                for (int x = position.X - (_radiusOfBrush * _radiusMultiply - y + position.Y);
                     x < position.X + _radiusOfBrush * _radiusMultiply - y + position.Y + 1;
                     x++)
                {
                    PlaceElementOnCoordinates(x, y, condition);
                }
            }
        }
        #endregion

        #region CheckOutOfBounds
        private static bool IsInsideOfWindow(Vector2i position)
        {
            return position.X < Frame.Window.Size.X && position.Y < Frame.Window.Size.Y &&
                   position is { X: >= 0, Y: >= 0 };
        }

        private static bool IsInsideOfGridAndWindow(Vector2i position)
        {
            return IsInsideOfWindow(position) && position.X < SizeX * CellSize && position.Y < SizeY * CellSize + PositionY &&
                   position.Y >= PositionY;
        }

        private static bool IsOutOfBoundsInGrid(int x, int y)
        {
            return x < 0 || y < 0 || x >= SizeX || y >= SizeY;
        }
        #endregion

        private void PlaceElementOnCoordinates(int x, int y, bool condition)
        {
            if (IsOutOfBoundsInGrid(x, y))
            {
                return;
            }

            Cells[y][x] = condition switch
            {
                true when Cells[y][x] is Air => ElementForPaint.Clone(),
                false when Cells[y][x] is not Air => new Air(Cells, DrawableCells),
                _ => Cells[y][x]
            };
        }

        public void Dispose()
        {
            Frame.Window.KeyPressed -= OnKeyPress;
            Frame.Window.KeyReleased -= OnKeyRelease;
        }
    }
    #endregion
}
