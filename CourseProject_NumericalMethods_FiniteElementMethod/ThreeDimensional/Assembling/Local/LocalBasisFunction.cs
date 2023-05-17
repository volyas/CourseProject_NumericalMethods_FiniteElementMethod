using CourseProject.Core.GridComponents;

namespace CourseProject.ThreeDimensional.Assembling.Local;

public class LocalBasisFunction
{
    private readonly Func<double, double> _xFunction;
    private readonly Func<double, double> _yFunction;
    private readonly Func<double, double> _zFunction;

    public LocalBasisFunction(Func<double, double> xFunction, Func<double, double> yFunction, Func<double, double> zFunction)
    {
        _xFunction = xFunction;
        _yFunction = yFunction;
        _zFunction = zFunction;
    }

    public double Calculate(Node3D node)
    {
        return _xFunction(node.X) * _yFunction(node.Y) * _zFunction(node.Z);
    }

    public double Calculate(double x, double y, double z)
    {
        return _xFunction(x) * _yFunction(y) * _zFunction(z);
    }
}