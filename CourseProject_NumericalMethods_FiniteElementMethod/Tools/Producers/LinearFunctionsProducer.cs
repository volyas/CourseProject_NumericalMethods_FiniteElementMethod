namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

public class LinearFunctionsProducer
{
    public Func<double, double> CreateFirstFunction(double rightCoordinate, double h)
    {
        return coordinate => (rightCoordinate - coordinate) / h;
    }

    public Func<double, double> CreateSecondFunction(double leftCoordinate, double h)
    {
        return coordinate => (coordinate - leftCoordinate) / h;
    }
}