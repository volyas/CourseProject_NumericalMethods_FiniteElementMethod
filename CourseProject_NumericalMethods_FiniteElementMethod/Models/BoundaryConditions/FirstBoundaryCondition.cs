namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.BoundaryConditions;

public class FirstBoundaryCondition
{
    public int[] GlobalNodesNumbers { get; set; }
    public double[] Us { get; set; }

    public FirstBoundaryCondition(int[] globalNodesNumbers, double[] us)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Us = us;
    }
}