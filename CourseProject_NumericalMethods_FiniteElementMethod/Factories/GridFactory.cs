using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

namespace CourseProject_NumericalMethods_FiniteElementMethod.Factories;

public class GridFactory
{
    private readonly GridComponentsProducer _gridComponentsProducer;

    public GridFactory(GridComponentsProducer gridComponentsProducer)
    {
        _gridComponentsProducer = gridComponentsProducer;
    }

    public Grid CreateGrid(Node[] cornerNodes, int amtByLength, int amtByWidth, int amtByHeight)
    {
        var grid = new Grid(
            _gridComponentsProducer.CreateNodes(amtByLength, amtByWidth, amtByHeight, cornerNodes),
            _gridComponentsProducer.CreateElements(amtByLength, amtByWidth, amtByHeight, cornerNodes),
            cornerNodes,
            amtByLength,
            amtByWidth,
            amtByHeight);

        return grid;
    }
}