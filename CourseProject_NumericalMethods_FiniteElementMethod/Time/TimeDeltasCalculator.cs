namespace CourseProject.Time;

public class TimeDeltasCalculator
{
    public (double, double, double) CalculateForThreeLayer(double t1, double t2, double t3)
    {
        return (t1 - t2, t1 - t3, t2 - t3);
    }

    public (double, double, double, double, double, double) CalculateForFourLayer(double t1, double t2, double t3, double t4)
    {
        return (t1 - t2, t1 - t3, t1 - t4, t2 - t3, t2 - t4, t3 - t4);
    }
}