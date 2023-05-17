using CourseProject.Core;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;
using CourseProject.GridGenerator.Area.Core;
using CourseProject.ThreeDimensional.Assembling.Local;

namespace CourseProject.ThreeDimensional;

public class FEMSolution
{
    private readonly Grid<Node3D> _grid;
    private readonly GlobalVector[] _solutions;
    private readonly double[] _timeLayers;
    private readonly LocalBasisFunctionsProvider _basisFunctionsProvider;

    public FEMSolution(Grid<Node3D> grid, GlobalVector[] solutions, double[] timeLayers, LocalBasisFunctionsProvider basisFunctionsProvider)
    {
        _grid = grid;
        _solutions = solutions;
        _timeLayers = timeLayers;
        _basisFunctionsProvider = basisFunctionsProvider;
    }

    public double Calculate(Node3D point, double time)
    {
        if (TimeLayersHas(time) && AreaHas(point))
        {
            var currentTimeLayerIndex = FindCurrentTimeLayer(time);

            var element = _grid.Elements.First(x => ElementHas(x, point));

            var basisFunctions = _basisFunctionsProvider.GetTrilinearFunctions(element);

            var lagrangePolynomials = CreateLagrangePolynomials(currentTimeLayerIndex);

            var sum = 0d;
            for (var j = 0; j < lagrangePolynomials.Length; j++)
            {
                sum += element.NodesIndexes
                    .Select((t, i) => _solutions[currentTimeLayerIndex - j][t] * basisFunctions[i].Calculate(point))
                    .Sum() * lagrangePolynomials[j](time);
            }

            //CourseHolder.WriteSolution(point, time, sum);

            return sum;
        }

        CourseHolder.WriteAreaInfo();
        CourseHolder.WriteSolution(point, double.NaN, double.NaN);
        return double.NaN;
    }

    public double CalcError(Func<Node3D, double, double> u, double time)
    {
        var solution = new GlobalVector(_solutions[0].Count);
        var trueSolution = new GlobalVector(_solutions[0].Count);

        for (var i = 0; i < _solutions[0].Count; i++)
        {
            solution[i] = Calculate(_grid.Nodes[i], time);
            trueSolution[i] = u(_grid.Nodes[i], time);
        }

        GlobalVector.Subtract(solution, trueSolution);

        return trueSolution.Norm;
    }

    private bool ElementHas(Element element, Node3D node)
    {
        var leftCornerNode = _grid.Nodes[element.NodesIndexes[0]];
        var rightCornerNode = _grid.Nodes[element.NodesIndexes[^1]];
        return node.X >= leftCornerNode.X && node.Y >= leftCornerNode.Y && node.Z >= leftCornerNode.Z &&
               node.X <= rightCornerNode.X && node.Y <= rightCornerNode.Y && node.Z <= rightCornerNode.Z;
        ;
    }

    private bool AreaHas(Node3D node)
    {
        var leftCornerNode = _grid.Nodes[0];
        var rightCornerNode = _grid.Nodes[^1];
        return node.X >= leftCornerNode.X && node.Y >= leftCornerNode.Y && node.Z >= leftCornerNode.Z &&
               node.X <= rightCornerNode.X && node.Y <= rightCornerNode.Y && node.Z <= rightCornerNode.Z;
    }

    private bool TimeLayersHas(double time)
    {
        var interval = new Interval(_timeLayers[0], _timeLayers[^1]);
        return interval.Has(time);
    }

    private int FindCurrentTimeLayer(double time)
    {
        return Array.FindIndex(_timeLayers, x => time <= x);
    }

    private Func<double, double>[] CreateLagrangePolynomials(int timeLayerIndex)
    {
        switch (timeLayerIndex)
        {
            case 1:
                {
                    var currentTime = _timeLayers[timeLayerIndex];
                    var previousTime = _timeLayers[timeLayerIndex - 1];
                    var values = new Func<double, double>[]
                    {
                    t => (t - currentTime) / (previousTime - currentTime),
                    t => (t - previousTime) / (currentTime - previousTime)
                    };

                    return values;
                }
            case 2:
                {
                    var currentTime = _timeLayers[timeLayerIndex];
                    var previousTime = _timeLayers[timeLayerIndex - 1];
                    var twoLayersBackTime = _timeLayers[timeLayerIndex - 2];
                    var values = new Func<double, double>[]
                    {
                    t => (t - twoLayersBackTime) * (t - previousTime) /
                        ((currentTime - twoLayersBackTime) * (currentTime - previousTime)),
                    t => -(t - twoLayersBackTime) * (t - currentTime) /
                        ((currentTime - previousTime) * (previousTime - twoLayersBackTime)),
                    t => (t - previousTime) * (t - currentTime) /
                        ((currentTime - twoLayersBackTime) * (previousTime - twoLayersBackTime)),
                    };

                    return values;
                }
            default:
                {
                    var currentTime = _timeLayers[timeLayerIndex];
                    var previousTime = _timeLayers[timeLayerIndex - 1];
                    var twoLayersBackTime = _timeLayers[timeLayerIndex - 2];
                    var threeLayersBackTime = _timeLayers[timeLayerIndex - 3];

                    var values = new Func<double, double>[]
                    {
                    t => (t - threeLayersBackTime) * (t - twoLayersBackTime) * (t - previousTime) /
                         ((currentTime - threeLayersBackTime) * (currentTime - twoLayersBackTime) *
                         (currentTime - previousTime)),
                    t => (t - threeLayersBackTime) * (t - twoLayersBackTime) * (t - currentTime) /
                         ((previousTime - threeLayersBackTime) * (previousTime - twoLayersBackTime) *
                         (previousTime - currentTime)),
                    t => (t - threeLayersBackTime) * (t - previousTime) * (t - currentTime) /
                         ((twoLayersBackTime - threeLayersBackTime) * (twoLayersBackTime - previousTime) *
                         (twoLayersBackTime - currentTime)),
                    t => (t - twoLayersBackTime) * (t - previousTime) * (t - currentTime) /
                         ((threeLayersBackTime - twoLayersBackTime) * (threeLayersBackTime - previousTime) *
                         (threeLayersBackTime - currentTime))
                    };

                    return values;
                }
        }
    }
}