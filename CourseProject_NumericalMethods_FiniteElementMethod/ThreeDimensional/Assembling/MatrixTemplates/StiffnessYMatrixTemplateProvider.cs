using CourseProject.Core.Base;
using CourseProject.FEM.Assembling;

namespace CourseProject.ThreeDimensional.Assembling.MatrixTemplates;

public class StiffnessYMatrixTemplateProvider : ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix()
    {
        return new BaseMatrix(new double[,]
        {
            { 4, 2, -4, -2, 2, 1, -2, -1 },
            { 2, 4, -2, -4, 1, 2, -1, -2 },
            { -4, -2, 4, 2, -2, -1, 2, 1},
            { -2, -4, 2, 4, -1, -2, 1, 2 },
            { 2, 1, -2, -1, 4, 2, -4, -2 },
            { 1, 2, -1, -2, 2, 4, -2, -4 },
            { -2, -1, 2, 1, -4, -2, 4, 2 },
            { -1, -2, 1, 2, -2, -4, 2, 4 }
        });
    }
}