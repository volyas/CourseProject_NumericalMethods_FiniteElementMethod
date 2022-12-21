using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.SLAESolution;

public class SLAESolver
{
    public static GlobalVector SolveSLAE(GlobalMatrix globalMatrix, GlobalVector b)
    {
        var y = CalcY(globalMatrix, b);
        var x = CalcX(globalMatrix, y);
        return x;
    }
    public static GlobalVector CalcY(GlobalMatrix globalMatrix, GlobalVector b)
    {
        var n = globalMatrix.N;
        var ig = globalMatrix.IG;
        var jg = globalMatrix.JG;
        var gg = globalMatrix.GG;
        var di = globalMatrix.DI;

        var y = new GlobalVector(n);

        for (var i = 0; i < n; i++)
        {
            var sum = 0.0;
            for (var j = ig[i]; j < ig[i + 1]; j++)
            {
                sum += gg[j] * y[jg[j]];
            }
            y[i] = (b[i] - sum) / di[i];
        }

        return y;
    }

    public static GlobalVector CalcX(GlobalMatrix sparseMatrix, GlobalVector y)
    {
        var n = sparseMatrix.N;
        var ig = sparseMatrix.IG;
        var jg = sparseMatrix.JG;
        var gg = sparseMatrix.GG;
        var di = sparseMatrix.DI;

        var x = (GlobalVector)y.Clone();

        for (var i = n - 1; i >= 0; i--)
        {
            x[i] /= di[i];
            for (var j = ig[i + 1] - 1; j >= ig[i]; j--)
            {
                x[jg[j]] -= gg[j] * x[i];
            }
        }

        return x;
    }
}