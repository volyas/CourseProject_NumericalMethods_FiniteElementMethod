using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;

public class GlobalMatrix
{
    public int N { get; set; }
    public double[] DI { get; set; }
    public double[] GG { get; set; }
    public int[] JG { get; set; }
    public int[] IG { get; set; }

    public GlobalMatrix() { }

    public GlobalMatrix(AdjacencyList adjacencyList)
    {
        var list = adjacencyList.List;
        N = list.Count;
        DI = new double[list.Count];
        JG = list.SelectMany(nodeList => nodeList).ToArray();
        GG = new double[JG.Length];
        var amount = 0;
        var buf = list.Select(nodeList => amount += nodeList.Count).ToList();
        buf.Insert(0, 0);
        IG = buf.ToArray();
    }

    public static GlobalVector operator *(GlobalMatrix globalMatrix, GlobalVector globalVector)
    {
        var n = globalMatrix.N;
        var ig = globalMatrix.IG;
        var jg = globalMatrix.JG;
        var gg = globalMatrix.GG;
        var di = globalMatrix.DI;

        var result = new GlobalVector(n);

        for (var i = 0; i < n; i++)
        {
            result[i] += di[i] * globalVector[i];
            for (var j = ig[i]; j < ig[i + 1]; j++)
            {
                result[i] += gg[j] * globalVector[jg[j]];
                result[jg[j]] += gg[j] * globalVector[i];
            }
        }

        return result;
    }

    public void PlaceLocalMatrix(LocalMatrix localMatrixA, int[] globalNodesNumbers)
    {
        for (var i = 0; i < globalNodesNumbers.Length; i++)
        {
            for (var j = 0; j < i; j++)
            {
                var elementIndex = Array.IndexOf(JG, globalNodesNumbers[j], IG[globalNodesNumbers[i]],
                    IG[globalNodesNumbers[i] + 1] - IG[globalNodesNumbers[i]]);
                GG[elementIndex] += localMatrixA[i, j];
            }

            DI[globalNodesNumbers[i]] += localMatrixA[i, i];
        }
    }
}