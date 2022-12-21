using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.SLAESolution;

public class IncompleteCholeskyDecomposition
{
    public static GlobalMatrix Decomposition(GlobalMatrix globalMatrix)
    {
        var n = globalMatrix.N;
        var cIG = new int[n + 1];
        Array.Copy(globalMatrix.IG, cIG, n + 1);
        var cJG = new int[cIG[n]];
        Array.Copy(globalMatrix.JG, cJG, cIG[n]);
        var cGG = new double[cIG[n]];
        Array.Copy(globalMatrix.GG, cGG, cIG[n]);
        var cDI = new double[n];
        Array.Copy(globalMatrix.DI, cDI, n);

        for (var i = 0; i < n; i++)
        {
            var sumD = 0.0;
            for (var j = cIG[i]; j < cIG[i + 1]; j++)
            {
                var sumIPrev = 0.0;
                for (var k = cIG[i]; k < j; k++)
                {
                    var iPrev = i - cJG[j];
                    var kPrev = Array.IndexOf(cJG, cJG[k], cIG[i - iPrev], cIG[i - iPrev + 1] - cIG[i - iPrev]);
                    if (kPrev != -1)
                    {
                        sumIPrev += cGG[k] * cGG[kPrev];
                    }
                }
                cGG[j] = (cGG[j] - sumIPrev) / cDI[cJG[j]];
                sumD += cGG[j] * cGG[j];
            }
            cDI[i] = Math.Sqrt(cDI[i] - sumD);
        }

        var choleskySparseMatrix = new GlobalMatrix
        {
            N = n,
            IG = cIG,
            JG = cJG,
            GG = cGG,
            DI = cDI
        };

        return choleskySparseMatrix;
    }
}