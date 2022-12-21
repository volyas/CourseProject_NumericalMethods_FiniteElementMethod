using System.Text;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools;

public class CourseHolder
{
    public static void GetInfo(int iteration, double residual)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Iteration number: " + iteration + ", ");

        var info = "residual: " + residual;
        stringBuilder.Append(info.Replace(',', '.'));

        stringBuilder.Append("                                   \r");

        Console.Write(stringBuilder.ToString());
    }
}