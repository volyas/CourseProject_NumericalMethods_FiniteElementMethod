using CourseProject.Core.Base;
using CourseProject.FEM.Assembling;

namespace CourseProject.ThreeDimensional.Assembling.MatrixTemplates;

public class StiffnessXMatrixTemplateProvider : ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix()
    {
        return new BaseMatrix(new double[,]
        {
            { 4, -4, 2, -2, 2, -2, 1, -1 },
            { -4, 4, -2, 2, -2, 2, -1, 1 },
            { 2, -2, 4, -4, 1, -1, 2, -2 },
            { -2, 2, -4, 4, -1, 1, -2, 2 },
            { 2, -2, 1, -1, 4, -4, 2, -2 },
            { -2, 2, -1, 1, -4, 4, -2, 2 },
            { 1, -1, 2, -2, 2, -2, 4, -4 },
            { -1, 1, -2, 2, -2, 2, -4, 4 }
        });
    }
}