using CourseProject.Core.Global;

namespace CourseProject.Time.Schemes.Explicit;

public class TwoLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;

    public TwoLayer(SymmetricSparseMatrix stiffnessMatrix, SymmetricSparseMatrix sigmaMassMatrix)
    {
        _stiffnessMatrix = stiffnessMatrix;
        _sigmaMassMatrix = sigmaMassMatrix;
    }

    public Equation<SymmetricSparseMatrix> BuildEquation
    (
        GlobalVector rightPart,
        GlobalVector previousSolution,
        double currentTime,
        double previousTime
    )
    {
        var delta0 = currentTime - previousTime;

        var matrixA = 1 / delta0 * _sigmaMassMatrix;
        var q = new GlobalVector(matrixA.CountRows);
        var b =
            GlobalVector.Sum
            (
                rightPart,
                GlobalVector.Subtract
                (
                    GlobalVector.Multiply(1 / delta0, _sigmaMassMatrix * previousSolution),
                    _stiffnessMatrix * previousSolution
                )
            );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}