using SFML.Graphics;

namespace Sandbox.insideOfGame.baseOfElements;

public abstract class UnmovableElement(IList<IList<Element>> parentCells, IList<IList<Color>> parentDrawableCells) : Element(parentCells, parentDrawableCells);
