using CourseProject.Core.Global;

namespace CourseProject.Time.Schemes.Explicit;

public class ThreeLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly TimeDeltasCalculator _timeDeltasCalculator;

    public ThreeLayer(SymmetricSparseMatrix stiffnessMatrix, SymmetricSparseMatrix sigmaMassMatrix,
        TimeDeltasCalculator timeDeltasCalculator)
    {
        _stiffnessMatrix = stiffnessMatrix;
        _sigmaMassMatrix = sigmaMassMatrix;
        _timeDeltasCalculator = timeDeltasCalculator;
    }

    public Equation<SymmetricSparseMatrix> BuildEquation
    (
        GlobalVector rightPart,
        GlobalVector previousSolution,
        GlobalVector twoLayersBackSolution,
        double currentTime,
        double previousTime,
        double twoLayersBackTime
    )
    {
        var (delta0, delta1, delta2) = _timeDeltasCalculator.CalculateForThreeLayer(currentTime, previousTime, twoLayersBackTime);

        var matrixA = delta2 / (delta0 * delta1) * _sigmaMassMatrix;
        var q = new GlobalVector(matrixA.CountRows);
        var b =
            GlobalVector.Sum
            (
                rightPart,
                GlobalVector.Subtract
                (
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply((-delta0 + delta2) / (delta0 * delta2), _sigmaMassMatrix * previousSolution),
                        GlobalVector.Multiply(delta0 / (delta1 * delta2), _sigmaMassMatrix * twoLayersBackSolution)
                    ),
                    _stiffnessMatrix * previousSolution
                )
            );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}