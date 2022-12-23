using System.Globalization;
using System.Text;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools;

public class CourseHolder
{
    private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-US");
    public static void GetInfo(int iteration, double residual)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Iteration number: " + iteration + ", ");

        var info = "residual: " + residual;
        stringBuilder.Append(info.Replace(',', '.'));

        stringBuilder.Append("                                   \r");

        Console.Write(stringBuilder.ToString());
    }
    public static void WriteSolution(Node node, double result)
    {
        Console.WriteLine($"Function value at the point ({node.X}, {node.Y}, {node.Z}) = {result.ToString("0.00000000000000e+00", _culture)}");
    }
    public static void WriteAreaInfo()
    {
        Console.WriteLine("The point is not in the calculation area! Choose another.");
    }
}