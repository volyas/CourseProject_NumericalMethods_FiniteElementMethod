using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class PointIO
{
    public PointIO() { }

    public Node ReadNodeFromConsole()
    {
        Console.WriteLine("Input coordinates of point to find function value in it: ");
        var coordinates = Console.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
        var point = new Node(coordinates[0], coordinates[1], coordinates[2]);
        return point;
    }
}