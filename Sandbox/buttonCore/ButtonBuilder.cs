using SFML.Graphics;
using SFML.System;

namespace Sandbox.buttonCore;

public class ButtonBuilder
{
    private string _text = "";
    private Color _color = Color.White;
    private Vector2f _position = new Vector2f(0, 0);
    private Vector2f Size { get; set; } = new Vector2f(100, 100);

    private ButtonBuilder()
    {
    }

    public static ButtonBuilder Create() => new ButtonBuilder();

    public Button Build(Action actionOnClick) => new Button(_position, Size, _text, _color, actionOnClick);

    public ButtonBuilder SetText(string text)
    {
        _text = text;
        return this;
    }

    public ButtonBuilder SetColor(Color color)
    {
        _color = color;
        return this;
    }

    public ButtonBuilder SetPosition(Vector2f position)
    {
        _position = position;
        return this;
    }

    public ButtonBuilder SetSize(Vector2f size)
    {
        Size = size;
        return this;
    }
}
