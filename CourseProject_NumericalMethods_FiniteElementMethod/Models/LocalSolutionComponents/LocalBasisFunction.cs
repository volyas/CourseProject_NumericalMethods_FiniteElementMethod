namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

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

    public double CalcFunction(double x, double y, double z)
    {
        return _xFunction(x) * _yFunction(y) * _zFunction(z);
    }
}