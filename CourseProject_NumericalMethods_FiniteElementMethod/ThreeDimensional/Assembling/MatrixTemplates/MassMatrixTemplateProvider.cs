using CourseProject.Core.Base;
using CourseProject.FEM.Assembling;

namespace CourseProject.ThreeDimensional.Assembling.MatrixTemplates;

public class MassMatrixTemplateProvider : ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix()
    {
        return new BaseMatrix(
            new double[,]
            {
                { 8, 4, 4, 2, 4, 2, 2, 1 },
                { 4, 8, 2, 4, 2, 4, 1, 2 },
                { 4, 2, 8, 4, 2, 1, 4, 2 },
                { 2, 4, 4, 8, 1, 2, 2, 4 },
                { 4, 2, 2, 1, 8, 4, 4, 2 },
                { 2, 4, 1, 2, 4, 8, 2, 4 },
                { 2, 1, 4, 2, 4, 2, 8, 4 },
                { 1, 2, 2, 4, 2, 4, 4, 8 }
            }
        );
    }
}