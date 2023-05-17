using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;

namespace CourseProject.FEM.Assembling.Local;

public interface ILocalAssembler
{
    public LocalMatrix AssembleStiffnessMatrix(Element element);
    public LocalMatrix AssembleSigmaMassMatrix(Element element);
    public LocalVector AssembleRightSide(Element element, double time);
}