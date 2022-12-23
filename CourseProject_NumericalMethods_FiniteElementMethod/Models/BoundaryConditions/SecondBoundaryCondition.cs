namespace CourseProject_NumericalMethods_FiniteElementMethod.Models.BoundaryConditions;

public class SecondBoundaryCondition
{
    public int[] GlobalNodesNumbers { get; set; }
    public double[] Thetas { get; set; }
 

    public SecondBoundaryCondition(int[] globalNodesNumbers, double[] thetas)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Thetas = thetas;
      
    }
}