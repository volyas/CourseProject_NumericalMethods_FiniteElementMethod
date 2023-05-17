using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.BoundaryConditions;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;
using CourseProject.ThreeDimensional.Parameters;

namespace CourseProject.ThreeDimensional.Assembling.Boundary;

public class SecondBoundaryProvider
{
    private readonly Grid<Node3D> _grid;
    private readonly Func<Node3D, double, double> _u;
    private readonly MaterialFactory _materialFactory;
    private readonly BaseMatrix _templateMatrix;
    private readonly DerivativeCalculator _derivativeCalculator;

    public SecondBoundaryProvider(Grid<Node3D> grid, Func<Node3D, double, double> u, MaterialFactory materialFactory,
        ITemplateMatrixProvider templateMatrixProvider, DerivativeCalculator derivativeCalculator)
    {
        _grid = grid;
        _u = u;
        _materialFactory = materialFactory;
        _templateMatrix = templateMatrixProvider.GetMatrix();
        _derivativeCalculator = derivativeCalculator;
    }

    public SecondCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double time)
    {
        var conditions = new List<SecondCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, hs) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            BaseVector tetas;

            var lambda = _materialFactory.GetById(_grid.Elements[elementsIndexes[i]].MaterialId).Lambda;

            switch (bounds[i])
            {
                case Bound.Left:
                case Bound.Right:
                    tetas = GetXVector(indexes, bounds[i], lambda, time);
                    break;
                case Bound.Front:
                case Bound.Back:
                    tetas = GetYVector(indexes, bounds[i], lambda, time);
                    break;
                default:
                    tetas = GetZVector(indexes, bounds[i], lambda, time);
                    break;
            }

            var matrix = GetMatrix(hs[0], hs[1]);

            conditions.Add(new SecondCondition(new LocalVector(indexes, matrix * tetas)));
        }

        return conditions.ToArray();
    }

    private BaseVector GetXVector(int[] indexes, Bound bound, double lambda, double time)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'x'),
            [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'x'),
            [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'x'),
            [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'x'),
        };

        if (bound == Bound.Left)
        {
            BaseVector.Multiply(-lambda, vector);
        }
        else
        {
            BaseVector.Multiply(lambda, vector);
        }

        return vector;
    }

    private BaseVector GetYVector(int[] indexes, Bound bound, double lambda, double time)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'y'),
            [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'y'),
            [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'y'),
            [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'y'),
        };

        if (bound == Bound.Front)
        {
            BaseVector.Multiply(-lambda, vector);
        }
        else
        {
            BaseVector.Multiply(lambda, vector);
        }

        return vector;
    }

    private BaseVector GetZVector(int[] indexes, Bound bound, double lambda, double time)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'z'),
            [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'z'),
            [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'z'),
            [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'z'),
        };

        if (bound == Bound.Lower)
        {
            BaseVector.Multiply(-lambda, vector);
        }
        else
        {
            BaseVector.Multiply(lambda, vector);
        }

        return vector;
    }

    private BaseMatrix GetMatrix(double h1, double h2)
    {
        return h1 * h2 / 36d * _templateMatrix;
    }
}