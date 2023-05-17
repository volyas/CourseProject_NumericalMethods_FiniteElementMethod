using CourseProject.Core;
using CourseProject.Core.BoundaryConditions;
using CourseProject.Core.GridComponents;

namespace CourseProject.ThreeDimensional.Assembling.Boundary;

public class FirstBoundaryProvider
{
    private readonly Grid<Node3D> _grid;
    private readonly Func<Node3D, double, double> _u;

    public FirstBoundaryProvider(Grid<Node3D> grid, Func<Node3D, double, double> u)
    {
        _grid = grid;
        _u = u;
    }

    public FirstCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double time)
    {
        var conditions = new List<FirstCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, _) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            var values = new double[indexes.Length];

            for (var j = 0; j < indexes.Length; j++)
            {
                values[j] = _u(_grid.Nodes[indexes[i]], time);
            }

            conditions.Add(new FirstCondition(indexes, values));
        }

        return conditions.ToArray();
    }

    public (int[], Bound[]) GetConditions(int elementsByLength, int elementsByWidth, int elementsByHeight)
    {
        var elementsIndexes = new List<int>();
        var bounds = new List<Bound>();

        for (var i = 0; i < elementsByLength * elementsByWidth; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Lower);
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByLength; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j);
                bounds.Add(Bound.Front);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j * elementsByLength + (elementsByWidth - 1));
                bounds.Add(Bound.Right);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j * elementsByLength);
                bounds.Add(Bound.Left);
            }
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            for (var j = 0; j < elementsByWidth; j++)
            {
                elementsIndexes.Add(i * elementsByWidth * elementsByLength + j + elementsByLength * (elementsByWidth - 1));
                bounds.Add(Bound.Back);
            }
        }

        for (var i = elementsByWidth * elementsByLength * (elementsByHeight - 1); i < elementsByWidth * elementsByLength * elementsByHeight; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Upper);
        }

        return (elementsIndexes.ToArray(), bounds.ToArray());
    }
}