using CourseProject.Core.Global;

namespace CourseProject.SLAE.Preconditions;

public interface IPreconditioner<out TResult>
{
    public TResult Decompose(SymmetricSparseMatrix globalMatrix);
}