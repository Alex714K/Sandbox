namespace Sandbox;

public interface IClonable<out T>
{
    T Clone();
}
