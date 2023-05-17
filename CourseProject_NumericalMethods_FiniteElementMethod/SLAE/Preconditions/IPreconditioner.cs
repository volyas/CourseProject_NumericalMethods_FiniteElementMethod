namespace UMF3.SLAE.Preconditions;

public interface IPreconditioner<out TResult>
{
    public TResult Decompose(SparseMatrix globalMatrix);
}