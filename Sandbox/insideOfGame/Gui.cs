using Sandbox.buttonCore;
using Sandbox.insideOfGame.baseOfElements;
using SFML.Graphics;
using SFML.System;

namespace Sandbox.insideOfGame;

public class Gui : Drawable
{
    #region Buttons
    private readonly List<Button> _buttons = CreateButtons();

    private static List<Button> CreateButtons()
    {

        List<Element> elements = Grid.ElementsForCloning.OriginalElements;

        const int distanceBetweenButtons = 10;

        const int width = 120;
        const int height = 60;

        return elements.Select((element, i) =>
            ButtonBuilder.Create()
                .SetColor(element.OutsideColor)
                .SetText(element.GetType().Name)
                .SetPosition(new Vector2f(distanceBetweenButtons * (i + 1) + width * i, 20))
                .SetSize(new Vector2f(width, height))
                .Build(() => Grid.SetElementForPaint(element))
        ).ToList();
    }
    #endregion
    
    public void Draw(RenderTarget target, RenderStates states)
    {
        _buttons.ForEach(target.Draw);
    }
}
