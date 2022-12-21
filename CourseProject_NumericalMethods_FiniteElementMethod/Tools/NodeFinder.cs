using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools;

public class NodeFinder
{
    private readonly Grid _grid;

    public NodeFinder(Grid grid)
    {
        _grid = grid;
    }

    public Node FindNode(int nodeNumber)
    {
        return _grid.Nodes[nodeNumber];
    }
}