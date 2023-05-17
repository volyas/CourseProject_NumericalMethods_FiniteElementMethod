using CourseProject.Core.Global;

namespace CourseProject.SLAE.Solvers;

public interface ISolver<TMatrix>
{
    public GlobalVector Solve(Equation<TMatrix> equation);
}