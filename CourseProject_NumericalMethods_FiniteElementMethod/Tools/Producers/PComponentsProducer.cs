namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

public class PComponentsProducer
{
    private readonly Func<double, double, double, double> _f;

    private readonly NodeFinder _nodeFinder;

    public PComponentsProducer(Func<double, double, double, double> f, NodeFinder nodeFinder)
    {
        _f = f;
        _nodeFinder = nodeFinder;
    }

    public double CalcRightPart(int nodeNumber)
    {
        var node = _nodeFinder.FindNode(nodeNumber);
        return _f(node.X, node.Y, node.Z);
    }
}