using CourseProject.Core.GridComponents;

namespace CourseProject.FEM;

public class CourseHolder
{
    public static void GetInfo(int iteration, double residual)
    {
        Console.Write($"Iteration: {iteration}, residual: {residual:E14}                                   \r");
    }

    public static void WriteSolution(Node3D point, double time, double value)
    {
        Console.WriteLine($"({point.X},{point.Y},{point.Z}) {time} {value:E14}");
    }

    public static void WriteAreaInfo()
    {
        Console.WriteLine("Point not in area or time interval");
    }
}