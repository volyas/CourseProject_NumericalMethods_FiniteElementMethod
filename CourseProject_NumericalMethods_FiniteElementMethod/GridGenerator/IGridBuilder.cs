using CourseProject.Core;

namespace CourseProject.GridGenerator;

public interface IGridBuilder<TPoint>
{
    public Grid<TPoint> Build();
}