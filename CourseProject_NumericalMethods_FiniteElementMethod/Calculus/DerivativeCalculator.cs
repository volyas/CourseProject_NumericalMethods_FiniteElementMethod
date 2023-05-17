using CourseProject.Core.GridComponents;

namespace CourseProject.Calculus;

public class DerivativeCalculator
{
    private const double Delta = 1.0e-3;

    public double Calculate(Func<Node3D, double, double> function, Node3D point, double time, char variableChar)
    {
        var result = variableChar switch
        {
            'x' => function(point with { X = point.X + Delta }, time) -
                   function(point with { X = point.X - Delta }, time),
            'y' => function(point with { Y = point.Y + Delta }, time) -
                   function(point with { Y = point.Y - Delta }, time),
            _ => function(point with { Z = point.Z + Delta }, time) -
                 function(point with { Z = point.Z - Delta }, time)
        };
        return result / (2.0 * Delta);
    }
}