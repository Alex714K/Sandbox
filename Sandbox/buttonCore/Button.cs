using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Sandbox.buttonCore;

public sealed class Button : IDisposable,
    Drawable
{
    private static readonly RectangleShape Body = new RectangleShape();

    private const int CharacterSize = 40;

    private static readonly Text Text = new Text("", new Font(@"C:\Windows\Fonts\times.ttf"), CharacterSize)
    {
        FillColor = Color.Black
    };

    private readonly Vector2f _position;
    private readonly Vector2f _size;
    private readonly string _text;
    private readonly Color _color;
    private readonly Action _actionOnClick;

    public Button(Vector2f position, Vector2f size, string text, Color color, Action doOnClick)
    {
        _position = position;
        _size = size;
        _text = text;
        _color = color;
        _actionOnClick = doOnClick;

        Frame.Window.MouseButtonPressed += OnPress;
    }

    private void OnPress(object? sender, MouseButtonEventArgs e)
    {
        if (e.Button != Mouse.Button.Left)
        {
            return;
        }

        if (e.X < _position.X || e.Y < _position.Y ||
            e.X > _position.X + _size.X || e.Y > _position.Y + _size.Y)
        {
            return;
        }

        _actionOnClick();
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        Body.Position = _position;
        Body.Size = _size;
        Body.FillColor = _color;
        target.Draw(Body);

        const float centerCoefficient = 0.5f;

        FloatRect rect = Text.GetGlobalBounds();
        Text.DisplayedString = _text;
        Text.Origin = new Vector2f(rect.Width * centerCoefficient, rect.Height);
        Text.Position = Body.Position + Body.Size * centerCoefficient;
        target.Draw(Text);
    }

    public void Dispose()
    {
        Text.Dispose();
        Frame.Window.MouseButtonPressed -= OnPress;
    }
}
