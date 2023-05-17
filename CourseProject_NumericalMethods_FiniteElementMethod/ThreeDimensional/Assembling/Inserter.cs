using CourseProject.Core.Global;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;

namespace CourseProject.ThreeDimensional.Assembling;

public class Inserter : IInserter<SymmetricSparseMatrix>
{
    public void InsertMatrix(SymmetricSparseMatrix globalMatrix, LocalMatrix localMatrix)
    {
        var nodesIndexes = localMatrix.Indexes;

        for (var i = 0; i < nodesIndexes.Length; i++)
        {
            for (var j = 0; j < i; j++)
            {
                var elementIndex = globalMatrix[nodesIndexes[i], nodesIndexes[j]];

                if (elementIndex == -1) continue;
                globalMatrix.Values[elementIndex] += localMatrix[i, j];
            }

            globalMatrix.Diagonal[nodesIndexes[i]] += localMatrix[i, i];
        }
    }

    public void InsertVector(GlobalVector globalVector, LocalVector localVector)
    {
        for (var i = 0; i < localVector.Count; i++)
        {
            globalVector[localVector.Indexes[i]] += localVector[i];
        }
    }
}