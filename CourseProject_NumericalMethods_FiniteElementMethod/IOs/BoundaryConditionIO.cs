namespace CourseProject_NumericalMethods_FiniteElementMethod.IOs;

public class BoundaryConditionIO
{
    private readonly string _path;

    public BoundaryConditionIO(string path)
    {
        _path = path;
    }

    public void ReadFirstCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> usList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        usList = new List<double[]>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new ReadOnlySpan<string>(boundaryCondition, 0, 4).ToArray().Select(int.Parse).ToArray());
            usList.Add(new ReadOnlySpan<string>(boundaryCondition, 4, 4).ToArray().Select(double.Parse).ToArray());
        }
    }

    public void ReadSecondCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> thetasList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        thetasList = new List<double[]>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new ReadOnlySpan<string>(boundaryCondition, 0, 4).ToArray().Select(int.Parse).ToArray());
            thetasList.Add(new ReadOnlySpan<string>(boundaryCondition, 4, 4).ToArray().Select(double.Parse).ToArray());
        }
    }

    public void ReadThirdCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double> betasList, out List<double[]> usList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        usList = new List<double[]>();
        betasList = new List<double>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new ReadOnlySpan<string>(boundaryCondition, 0, 4).ToArray().Select(int.Parse).ToArray());
            betasList.Add(double.Parse(boundaryCondition[4]));
            usList.Add(new ReadOnlySpan<string>(boundaryCondition, 5, 4).ToArray().Select(double.Parse).ToArray());
        }
    }
}