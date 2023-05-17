using CourseProject.Core.BoundaryConditions;
using CourseProject.Core.Global;

namespace CourseProject.FEM.Assembling.Global;

public interface IGaussExcluder<TMatrix>
{
    public void Exclude(Equation<TMatrix> equation, FirstCondition condition);
}