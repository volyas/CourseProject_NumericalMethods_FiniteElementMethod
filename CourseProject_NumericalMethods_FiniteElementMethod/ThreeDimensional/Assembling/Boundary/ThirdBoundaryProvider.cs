using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.BoundaryConditions;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;
using CourseProject.ThreeDimensional.Parameters;

namespace CourseProject.ThreeDimensional.Assembling.Boundary;

public class ThirdBoundaryProvider
{
    private readonly Grid<Node3D> _grid;
    private readonly Func<Node3D, double, double> _u;
    private readonly MaterialFactory _materialFactory;
    private readonly BaseMatrix _templateMatrix;
    private readonly DerivativeCalculator _derivativeCalculator;

    public ThirdBoundaryProvider(Grid<Node3D> grid, Func<Node3D, double, double> u, MaterialFactory materialFactory,
        ITemplateMatrixProvider matrixProvider, DerivativeCalculator derivativeCalculator)
    {
        _grid = grid;
        _u = u;
        _materialFactory = materialFactory;
        _templateMatrix = matrixProvider.GetMatrix();
        _derivativeCalculator = derivativeCalculator;
    }

    public ThirdCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double[] betas, double time)
    {
        var conditions = new List<ThirdCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, hs) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);
            var lambda = _materialFactory.GetById(_grid.Elements[elementsIndexes[i]].MaterialId).Lambda;

            var uS = GetUs(indexes, bounds[i], lambda, betas[i], time);

            var matrix = GetMatrix(hs[0], hs[1], betas[i]);
            var vector = GetVector(matrix, uS);

            conditions.Add(new ThirdCondition(new LocalMatrix(indexes, matrix),
                new LocalVector(indexes, vector)));
        }

        return conditions.ToArray();
    }

    private BaseMatrix GetMatrix(double h1, double h2, double beta)
    {
        var matrix = beta * h1 * h2 / 36d * _templateMatrix;

        return matrix;
    }

    private BaseVector GetVector(BaseMatrix matrix, BaseVector uS)
    {
        return matrix * uS;
    }

    private BaseVector GetUs(int[] indexes, Bound bound, double lambda, double beta, double time)
    {
        BaseVector vector;
        switch (bound)
        {
            case Bound.Left:
            case Bound.Right:
                {
                    vector = new BaseVector(indexes.Length)
                    {
                        [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'x'),
                        [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'x'),
                        [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'x'),
                        [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'x'),
                    };

                    if (bound == Bound.Right) BaseVector.Multiply(lambda, vector);
                    else BaseVector.Multiply(-lambda, vector);
                    break;
                }
            case Bound.Front:
            case Bound.Back:
                {
                    vector = new BaseVector(indexes.Length)
                    {
                        [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'y'),
                        [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'y'),
                        [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'y'),
                        [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'y'),
                    };

                    if (bound == Bound.Back) BaseVector.Multiply(lambda, vector);
                    else BaseVector.Multiply(-lambda, vector);
                    break;
                }
            default:
                {
                    vector = new BaseVector(indexes.Length)
                    {
                        [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'z'),
                        [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'z'),
                        [2] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[2]], time, 'z'),
                        [3] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[3]], time, 'z'),
                    };

                    if (bound == Bound.Lower) BaseVector.Multiply(lambda, vector);
                    else BaseVector.Multiply(-lambda, vector);
                    break;
                }
        }

        for (var i = 0; i < vector.Count; i++)
        {
            vector[i] = (vector[i] + beta * _u(_grid.Nodes[indexes[i]], time)) / beta;
        }

        return vector;
    }

}