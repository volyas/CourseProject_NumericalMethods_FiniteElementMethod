using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools;

public class SolutionFinder
{
    private readonly Grid _grid;
    private readonly GlobalVector _globalVector;
    private readonly NodeFinder _nodeFinder;

    public SolutionFinder(Grid grid, GlobalVector globalVector, NodeFinder nodeFinder)
    {
        _grid = grid;
        _globalVector = globalVector;
        _nodeFinder = nodeFinder;
    }

    public double FindSolution(Node node)
    {
        var result = 0.0;
        foreach (var element in _grid.Elements)
        {
            if (!CheckInside(element, node)) continue;
            var i = 0;
            foreach (var globalNodeNumber in element.GlobalNodesNumbers)
            {
                if (_nodeFinder.FindNode(globalNodeNumber).Equals(node))
                {
                    result = _globalVector[globalNodeNumber];
                    return result;
                }
                result += element.LocalBasisFunctions[i++].CalcFunction(node.X, node.Y, node.Z) *
                          _globalVector[globalNodeNumber];
            }
            return result;
        }
        return result;
    }

    private bool CheckInside(Element element, Node node)
    {
        var leftCornerNode = _nodeFinder.FindNode(element.GlobalNodesNumbers[0]);
        var rightCornerNode = _nodeFinder.FindNode(element.GlobalNodesNumbers[^1]);
        return leftCornerNode.X <= node.X && node.X <= rightCornerNode.X &&
               leftCornerNode.Y <= node.Y && node.Y <= rightCornerNode.Y &&
               leftCornerNode.Z <= node.Z && node.Z <= rightCornerNode.Z;
    }
    public bool CheckArea(Node node)
    {
        var leftCornerNode = _grid.CornerNodes[0];
        var rightCornerNode = _grid.CornerNodes[1];
        return node.X >= leftCornerNode.X && node.X <= rightCornerNode.X &&
               node.Y >= leftCornerNode.Y && node.Y <= rightCornerNode.Y &&
               node.Z >= leftCornerNode.Z && node.Z <= rightCornerNode.Z;
    }
}