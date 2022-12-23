using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;
using System.Globalization;

namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class GlobalVectorIO
{
    private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-US");
    private readonly string _path;
    public GlobalVectorIO(string path)
    {
        _path = path;
    }

    public GlobalVector Read(string fileName)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var vectorValues = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
        var vector = new GlobalVector(vectorValues);

        return vector;
    }

    public void Write(string fileName, GlobalVector globalVector)
    {
        using var streamWriter = new StreamWriter(_path + fileName);
        foreach (var element in globalVector.Vector)
        {
            streamWriter.WriteLine(element.ToString("0.00000000000000e+00", _culture));
        }
    }
}