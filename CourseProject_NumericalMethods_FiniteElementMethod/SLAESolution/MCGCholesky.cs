using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools.Calculators;

namespace CourseProject_NumericalMethods_FiniteElementMethod.SLAESolution;

public class MCGCholesky
{
    private GlobalMatrix _choleskyGlobalMatrix;
    private void PrepareProcess(GlobalMatrix a, GlobalVector x0, GlobalVector b, out GlobalVector r0, out GlobalVector z0)
    {
        _choleskyGlobalMatrix = IncompleteCholeskyDecomposition.Decomposition(a);
        r0 = b - a * x0;
        z0 = SLAESolver.SolveSLAE(_choleskyGlobalMatrix, r0);
    }

    public GlobalVector Solve(GlobalMatrix a, GlobalVector x0, GlobalVector b, double eps, int maxIter)
    {
        PrepareProcess(a, x0, b, out var r0, out var z0);
        var x = IterationProcess(a, x0, b, eps, maxIter, r0, z0);
        return x;
    }

    private GlobalVector IterationProcess(GlobalMatrix globalMatrix, GlobalVector x0, GlobalVector b, double eps, int maxIter,
        GlobalVector r0, GlobalVector z0)
    {
        var x = x0;
        var r = r0;
        var z = z0;

        var bNorm = b.CalcNorm();
        var residual = r.CalcNorm() / bNorm;

        for (var i = 1; i <= maxIter && residual > eps; i++)
        {
            var Mr = SLAESolver.SolveSLAE(_choleskyGlobalMatrix, r);

            var scalarMrR = ScalarProductCalculator.CalcScalarProduct(Mr, r);

            var AxZ = globalMatrix * z;

            var alphaK = scalarMrR / ScalarProductCalculator.CalcScalarProduct(AxZ, z);

            var xNext = x + z * alphaK;

            var rNext = r - AxZ * alphaK;

            var MrNext = SLAESolver.SolveSLAE(_choleskyGlobalMatrix, rNext);

            var betaK = ScalarProductCalculator.CalcScalarProduct(MrNext, rNext) / scalarMrR;

            var zNext = SLAESolver.SolveSLAE(_choleskyGlobalMatrix, rNext) + z * betaK;

            residual = rNext.CalcNorm() / bNorm;

            x = xNext;
            r = rNext;
            z = zNext;

            CourseHolder.GetInfo(i, residual);
        }

        return x;
    }
}