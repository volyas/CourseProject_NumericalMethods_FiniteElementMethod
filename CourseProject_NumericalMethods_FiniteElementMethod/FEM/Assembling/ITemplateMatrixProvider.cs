using CourseProject.Core.Base;

namespace CourseProject.FEM.Assembling;

public interface ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix();
}