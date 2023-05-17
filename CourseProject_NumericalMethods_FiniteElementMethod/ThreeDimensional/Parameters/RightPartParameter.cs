using CourseProject.Core;
using CourseProject.Core.GridComponents;
using CourseProject.FEM.Parameters;

namespace CourseProject.ThreeDimensional.Parameters;

public class RightPartParameter : IFunctionalParameter
{
    private readonly Func<Node3D, double, double> _function;
    private readonly Grid<Node3D> _grid;

    public RightPartParameter(
        Func<Node3D, double, double> function,
        Grid<Node3D> grid
    )
    {
        _function = function;
        _grid = grid;
    }

    public double Calculate(int nodeNumber, double time)
    {
        var node = _grid.Nodes[nodeNumber];
        return _function(node, time);
    }

    public double Calculate(Node3D node, double time)
    {
        return _function(node, time);
    }
}