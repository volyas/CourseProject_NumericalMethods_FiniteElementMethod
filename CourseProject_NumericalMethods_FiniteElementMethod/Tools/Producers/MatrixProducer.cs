using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

public class MatrixProducer
{
    public static LocalMatrix M = new LocalMatrix(new double[,]
    {
        { 8, 4, 4, 2, 4, 2, 2, 1 },
        { 4, 8, 2, 4, 2, 4, 1, 2 },
        { 4, 2, 8, 4, 2, 1, 4, 2 },
        { 2, 4, 4, 8, 1, 2, 2, 4 },
        { 4, 2, 2, 1, 8, 4, 4, 2 },
        { 2, 4, 1, 2, 4, 8, 2, 4 },
        { 2, 1, 4, 2, 4, 2, 8, 4 },
        { 1, 2, 2, 4, 2, 4, 4, 8 }
    });

    public static LocalMatrix Gx = new LocalMatrix(new double[,]
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

    public static LocalMatrix Gy = new LocalMatrix(new double[,]
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

    public static LocalMatrix Gz = new LocalMatrix(new double[,]
    {
        { 4, 2, 2, 1, -4, -2, -2, -1 },
        { 2, 4, 1, 2, -2, -4, -1, -2 },
        { 2, 1, 4, 2, -2, -1, -4, -2 },
        { 1, 2, 2, 4, -1, -2, -2, -4 },
        { -4, -2, -2, -1, 4, 2, 2, 1 },
        { -2, -4, -1, -2, 2, 4, 1, 2 },
        { -2, -1, -4, -2, 2, 1, 4, 2 },
        { -1, -2, -2, -4, 1, 2, 2, 4 }
    });
}