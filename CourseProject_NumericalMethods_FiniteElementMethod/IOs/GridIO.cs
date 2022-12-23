using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;
using System.Text.Json;

namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class GridIO
{
    private readonly string _path;

    public GridIO() { }

    public GridIO(string path)
    {
        _path = path;
    }

    public void ReadParametersFromConsole(out Node[] cornerNodes, out int amtByLength, out int amtByWidth, out int amtByHeight)
    {
        Console.WriteLine("Input grid coordinate.");
        cornerNodes = new Node[2];

        Console.Write("Input bottom left point: ");
        var point = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
        cornerNodes[0] = new Node(point[0], point[1], point[2]);

        Console.Write("Input upper right point: ");
        point = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
        cornerNodes[1] = new Node(point[0], point[1], point[2]);

        Console.Write("Input amount by length: ");
        amtByLength = int.Parse(Console.ReadLine());

        Console.Write("Input amount by width: ");
        amtByWidth = int.Parse(Console.ReadLine());

        Console.Write("Input amount by height: ");
        amtByHeight = int.Parse(Console.ReadLine());
    }

    public void ReadParametersFromFile(string fileName, out Node[] cornerNodes, out int amtByLength, out int amtByWidth, out int amtByHeight)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);
            cornerNodes = new Node[2];

            var point = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
            cornerNodes[0] = new Node(point[0], point[1], point[2]);

            point = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
            cornerNodes[1] = new Node(point[0], point[1], point[2]);

            var amtByLengthWidthAndHeight = streamReader.ReadLine().Split(' ').Select(int.Parse).ToArray();
            amtByLength = amtByLengthWidthAndHeight[0];
            amtByWidth = amtByLengthWidthAndHeight[1];
            amtByHeight = amtByLengthWidthAndHeight[2];
        }
        catch (Exception)
        {
            throw new Exception("Can't read parameters from file!");
        }
    }

    public Grid ReadGridFromJson(string fileName)
    {
        using var fileStream = new FileStream(_path + fileName, FileMode.OpenOrCreate);
        var grid = JsonSerializer.Deserialize<Grid>(fileStream);
        if (grid == null) throw new Exception("Can't read grid from file");
        return grid;
    }

    public void WriteGridToJson(string fileName, Grid grid)
    {
        using var fileStream = new FileStream(_path + fileName, FileMode.OpenOrCreate);
        JsonSerializer.Serialize(fileStream, grid);
    }
}