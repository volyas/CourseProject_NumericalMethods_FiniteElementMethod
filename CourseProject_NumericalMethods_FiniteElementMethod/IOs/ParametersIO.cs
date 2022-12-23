namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class ParametersIO
{
    private readonly string _path;
    public ParametersIO(string path)
    {
        _path = path;
    }
    public (double, int) ReadMethodParameters(string fileName)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var paramsIn = streamReader.ReadLine().Replace('.', ',').Split(' ');
        var parameters = (double.Parse(paramsIn[0]), int.Parse(paramsIn[1]));
        return parameters;
    }
}