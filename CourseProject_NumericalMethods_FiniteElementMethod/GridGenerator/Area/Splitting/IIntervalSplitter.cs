using CourseProject.GridGenerator.Area.Core;

namespace CourseProject.GridGenerator.Area.Splitting;

public interface IIntervalSplitter
{
    public IEnumerable<double> EnumerateValues(Interval interval);
    public int Steps { get; }
}