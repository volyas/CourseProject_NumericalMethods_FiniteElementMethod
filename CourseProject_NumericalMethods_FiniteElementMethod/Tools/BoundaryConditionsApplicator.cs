using CourseProject_NumericalMethods_FiniteElementMethod.Models.BoundaryConditions;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GlobalSolutionComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools;

public class BoundaryConditionsApplicator
{
    
    private readonly LocalMatrix _c1 = new LocalMatrix(new double[,]
    {
        { 4, 2, 2, 1 },
        { 2, 4, 1, 2 },
        { 2, 1, 4, 2 },
        { 1, 2, 2, 4 }
    });
    private readonly NodeFinder _nodeFinder;
    private const double Eps = 1.0e-16;

    public BoundaryConditionsApplicator(NodeFinder nodeFinder)
    {
        _nodeFinder = nodeFinder;
    }
    private static double CalcH(double coordinate1, double coordinate2)
    {
        return coordinate2 - coordinate1;
    }
    public void ApplyFirstCondition(GlobalMatrix globalMatrix, GlobalVector globalVector, FirstBoundaryCondition firstBoundaryCondition)
    {
        for (var i = 0; i < firstBoundaryCondition.GlobalNodesNumbers.Length; i++)
        {
            globalVector[firstBoundaryCondition.GlobalNodesNumbers[i]] = firstBoundaryCondition.Us[i];
            globalMatrix.DI[firstBoundaryCondition.GlobalNodesNumbers[i]] = 1.0;

            for (var j = globalMatrix.IG[firstBoundaryCondition.GlobalNodesNumbers[i]]; j < globalMatrix.IG[firstBoundaryCondition.GlobalNodesNumbers[i] + 1]; j++)
            {
                globalVector[globalMatrix.JG[j]] -= globalMatrix.GG[j] * firstBoundaryCondition.Us[i];
                globalMatrix.GG[j] = 0.0;
            }

            for (var j = firstBoundaryCondition.GlobalNodesNumbers[i] + 1; j < globalMatrix.N; j++)
            {
                var columnIndex = Array.IndexOf(globalMatrix.JG, firstBoundaryCondition.GlobalNodesNumbers[i], globalMatrix.IG[j], globalMatrix.IG[j + 1] - globalMatrix.IG[j]);
                if (columnIndex == -1) continue;

                globalVector[j] -= globalMatrix.GG[columnIndex] * firstBoundaryCondition.Us[i];
                globalMatrix.GG[columnIndex] = 0.0;
            }
        }
    }

    public void ApplySecondCondition(GlobalVector globalVector, SecondBoundaryCondition secondBoundaryCondition)
    {
        var firstNode = _nodeFinder.FindNode(secondBoundaryCondition.GlobalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(secondBoundaryCondition.GlobalNodesNumbers[3]);
        var h1 = 0.0;
        var h2 = 0.0;
        if (Math.Abs(secondNode.X - firstNode.X) < Eps)
        {
            h1 = CalcH(firstNode.Y, secondNode.Y);
            h2 = CalcH(firstNode.Z, secondNode.Z);
        }
        else if (Math.Abs(secondNode.Y - firstNode.Y) < Eps)
        {
            h1 = CalcH(firstNode.X, secondNode.X);
            h2 = CalcH(firstNode.Z, secondNode.Z);

        }
        else if (Math.Abs(secondNode.Z - firstNode.Z) < Eps)
        {
            h1 = CalcH(firstNode.X, secondNode.X);
            h2 = CalcH(firstNode.Y, secondNode.Y);

        }

        var matrixC = _c1 * ( h1 * h2 / 36.0);
        var vector = new LocalVector(4)
        {
            [0] = secondBoundaryCondition.Thetas[0],
            [1] = secondBoundaryCondition.Thetas[1],
            [2] = secondBoundaryCondition.Thetas[2],
            [3] = secondBoundaryCondition.Thetas[3]
        };
        vector = matrixC * vector;

        globalVector.PlaceLocalVector(vector, secondBoundaryCondition.GlobalNodesNumbers);
        
    }

    public void ApplyThirdCondition(GlobalMatrix globalMatrix, GlobalVector globalVector, ThirdBoundaryCondition thirdBoundaryCondition)
    {
        var firstNode = _nodeFinder.FindNode(thirdBoundaryCondition.GlobalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(thirdBoundaryCondition.GlobalNodesNumbers[3]);
        var h1 = 0.0;
        var h2 = 0.0;
        if (Math.Abs(secondNode.X - firstNode.X) < Eps)
        {
            h1 = CalcH(firstNode.Y, secondNode.Y);
            h2 = CalcH(firstNode.Z, secondNode.Z);
        }
        else if (Math.Abs(secondNode.Y - firstNode.Y) < Eps)
        {
            h1 = CalcH(firstNode.X, secondNode.X);
            h2 = CalcH(firstNode.Z, secondNode.Z);

        }
        else if (Math.Abs(secondNode.Z - firstNode.Z) < Eps)
        {
            h1 = CalcH(firstNode.X, secondNode.X);
            h2 = CalcH(firstNode.Y, secondNode.Y);
        }
        var matrixC = _c1 * (thirdBoundaryCondition.Beta * h1 * h2 / 36.0);
        globalMatrix.PlaceLocalMatrix(matrixC, thirdBoundaryCondition.GlobalNodesNumbers);

        var vector = new LocalVector(4)
        {
            [0] = thirdBoundaryCondition.Us[0],
            [1] = thirdBoundaryCondition.Us[1],
            [2] = thirdBoundaryCondition.Us[2],
            [3] = thirdBoundaryCondition.Us[3]
        };
        vector = matrixC * vector;

        globalVector.PlaceLocalVector(vector, thirdBoundaryCondition.GlobalNodesNumbers);
    }
    
}