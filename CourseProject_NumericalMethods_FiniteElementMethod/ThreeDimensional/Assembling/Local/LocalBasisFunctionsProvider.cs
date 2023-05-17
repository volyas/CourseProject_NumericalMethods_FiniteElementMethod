using CourseProject.Core;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;

namespace CourseProject.ThreeDimensional.Assembling.Local;

public class LocalBasisFunctionsProvider
{
    private readonly Grid<Node3D> _grid;
    private readonly LinearFunctionsProvider _linearFunctionsProvider;

    public LocalBasisFunctionsProvider(Grid<Node3D> grid, LinearFunctionsProvider linearFunctionsProvider)
    {
        _grid = grid;
        _linearFunctionsProvider = linearFunctionsProvider;
    }

    public LocalBasisFunction[] GetTrilinearFunctions(Element element)
    {
        var firstXFunction =
            _linearFunctionsProvider.CreateFirstFunction(_grid.Nodes[element.NodesIndexes[1]].X, element.Length);
        var secondXFunction =
            _linearFunctionsProvider.CreateSecondFunction(_grid.Nodes[element.NodesIndexes[0]].X, element.Length);
        var firstYFunction =
            _linearFunctionsProvider.CreateFirstFunction(_grid.Nodes[element.NodesIndexes[2]].Y, element.Height);
        var secondYFunction =
            _linearFunctionsProvider.CreateSecondFunction(_grid.Nodes[element.NodesIndexes[0]].Y, element.Height);
        var firstZFunction =
            _linearFunctionsProvider.CreateFirstFunction(_grid.Nodes[element.NodesIndexes[4]].Z, element.Height);
        var secondZFunction =
            _linearFunctionsProvider.CreateSecondFunction(_grid.Nodes[element.NodesIndexes[0]].Z, element.Height);

        var basisFunctions = new LocalBasisFunction[]
        {
            new (firstXFunction, firstYFunction, firstZFunction),
            new (secondXFunction, firstYFunction, firstZFunction),
            new (firstXFunction, secondYFunction, firstZFunction),
            new (secondXFunction, secondYFunction, firstZFunction),
            new (firstXFunction, firstYFunction, secondZFunction),
            new (secondXFunction, firstYFunction, secondZFunction),
            new (firstXFunction, secondYFunction, secondZFunction),
            new (secondXFunction, secondYFunction, secondZFunction),
        };

        return basisFunctions;
    }
}