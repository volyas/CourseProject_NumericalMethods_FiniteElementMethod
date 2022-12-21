using CourseProject_NumericalMethods_FiniteElementMethod.Factories;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.GridComponents;
using CourseProject_NumericalMethods_FiniteElementMethod.Models.LocalSolutionComponents;


namespace CourseProject_NumericalMethods_FiniteElementMethod.Tools.Producers;

public class GridComponentsProducer
{
    private readonly MaterialFactory _materialFactory;
    private readonly LinearFunctionsProducer _linearFunctionsProducer;

    public GridComponentsProducer(MaterialFactory materialFactory, LinearFunctionsProducer linearFunctionsProducer)
    {
        _materialFactory = materialFactory;
        _linearFunctionsProducer = linearFunctionsProducer;
    }

    public Node[] CreateNodes(int amtByLength, int amtByWidth, int amtByHeight, Node[] cornerNodes)
    {
        var nodes = new Node[(amtByLength + 1) * (amtByWidth + 1) * (amtByHeight + 1)];
        var length = cornerNodes[1].X - cornerNodes[0].X;
        var width = cornerNodes[1].Y - cornerNodes[0].Y;
        var height = cornerNodes[1].Z - cornerNodes[0].Z;

        var elementLength = length / amtByLength;
        var elementWidth = width / amtByWidth;
        var elementHeight = height / amtByHeight;

        for (var z = 0; z < amtByHeight + 1; z++)
        {
            for (var y = 0; y < amtByWidth + 1; y++)
            {
                for (var x = 0; x < amtByLength + 1; x++)
                {
                    nodes[x + y * (amtByLength + 1) + z * (amtByLength + 1) * (amtByWidth + 1)] = new Node(cornerNodes[0].X + elementLength * x, cornerNodes[0].Y + elementWidth * y, cornerNodes[0].Z + elementHeight * z);
                }
            }
        }
        
        return nodes;
    }
    public Element[] CreateElements(int amtByLength, int amtByWidth, int amtByHeight, Node[] cornerNodes)
    {
        var elements = new Element[amtByLength * amtByWidth * amtByHeight];

        var length = cornerNodes[1].X - cornerNodes[0].X;
        var width = cornerNodes[1].Y - cornerNodes[0].Y;
        var height = cornerNodes[1].Z - cornerNodes[0].Z;

        var elementLength = length / amtByLength;
        var elementWidth = width / amtByWidth;
        var elementHeight = height / amtByWidth;

        for (int z = 0; z < amtByHeight; z++)
        {
            for (int y = 0; y < amtByWidth; y++)
            {
                for (int x = 0; x < amtByLength; x++)
                {
                    //var nodes = new Node[]
                    //{
                    //        new(cornerNodes[0].X + amtByLength * x,
                    //            cornerNodes[0].Y + amtByLength * y,
                    //            cornerNodes[0].Z + amtByLength * z),

                    //        new(cornerNodes[0].X + amtByLength * (x + 1),
                    //            cornerNodes[0].Y + amtByLength * y,
                    //            cornerNodes[0].Z + amtByLength * z),

                    //        new(cornerNodes[0].X + amtByLength * x,
                    //            cornerNodes[0].Y + amtByLength * (y + 1),
                    //            cornerNodes[0].Z + amtByLength * z),

                    //        new(cornerNodes[0].X + amtByLength * (x + 1),
                    //            cornerNodes[0].Y + amtByLength * (y + 1),
                    //            cornerNodes[0].Z + amtByLength * z),
                                                   
                    //        new(cornerNodes[0].X + amtByLength * x,
                    //            cornerNodes[0].Y + amtByLength * y,
                    //            cornerNodes[0].Z + amtByLength * (z + 1)),
                                                   
                    //        new(cornerNodes[0].X + amtByLength * (x + 1),
                    //            cornerNodes[0].Y + amtByLength * y,
                    //            cornerNodes[0].Z + amtByLength * (z + 1)),
                                                   
                    //        new(cornerNodes[0].X + amtByLength * x,
                    //            cornerNodes[0].Y + amtByLength * (y + 1),
                    //            cornerNodes[0].Z + amtByLength * (z + 1)),
                                                   
                    //        new(cornerNodes[0].X + amtByLength * (x + 1),
                    //            cornerNodes[0].Y + amtByLength * (y + 1),
                    //            cornerNodes[0].Z + amtByLength * (z + 1))
                    //};

                    var globalNodesNumbers = new[]
                    {
                            x + y * (amtByLength + 1) + z * ((amtByLength + 1) * (amtByWidth + 1)),
                            (x + 1)  + y * (amtByLength + 1) + z * ((amtByLength + 1) * (amtByWidth + 1)),
                            x + (y + 1) * (amtByLength + 1) + z * ((amtByLength + 1) * (amtByWidth + 1)),
                            (x + 1) + (y + 1) * (amtByLength + 1) + z * ((amtByLength + 1) * (amtByWidth + 1)),
                            x + y * (amtByLength + 1) + (z + 1) * ((amtByLength + 1) * (amtByWidth + 1)),
                            (x + 1) + y * (amtByLength + 1) + (z + 1) * ((amtByLength + 1) * (amtByWidth + 1)),
                            x + (y + 1) * (amtByLength + 1) + (z + 1) * ((amtByLength + 1) * (amtByWidth + 1)),
                            (x + 1) + (y + 1) * (amtByLength + 1) + (z + 1) * ((amtByLength + 1) * (amtByWidth + 1))
                    };

                    var localBasisFunctions = new LocalBasisFunction[]
                    {
                        new(_linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].X + elementLength * (x + 1), elementLength),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Y + elementWidth * (y + 1), elementWidth),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (z + 1), elementHeight)),

                        new(_linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].X + elementLength * x, elementLength),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Y + elementWidth * (y + 1), elementWidth),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (z + 1), elementHeight)),

                        new(_linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].X + elementLength * (x + 1), elementLength),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Y + elementWidth * y, elementWidth),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (z + 1), elementHeight)),

                        new(_linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].X + elementLength * x, elementLength),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Y + elementWidth * y, elementWidth),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (z + 1), elementHeight)),

                        new(_linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].X + elementLength * (x + 1), elementLength),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Y + elementWidth * (y + 1), elementWidth),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Z + elementHeight * z, elementHeight)),

                        new(_linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].X + elementLength * x, elementLength),
                            _linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].Y + elementWidth * (y + 1), elementWidth),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Z + elementHeight * z, elementHeight)),

                        new(_linearFunctionsProducer.CreateFirstFunction(cornerNodes[0].X + elementLength * (x + 1), elementLength),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Y + elementWidth * y, elementWidth),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Z + elementHeight * z, elementHeight)),

                        new(_linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].X + elementLength * x, elementLength),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Y + elementWidth * y, elementWidth),
                            _linearFunctionsProducer.CreateSecondFunction(cornerNodes[0].Z + elementHeight * z, elementHeight))
                    };

                    var material = _materialFactory.CreateMaterial(0);
                    var element = new Element(globalNodesNumbers, material, localBasisFunctions);

                    elements[x + y * amtByLength + z * amtByLength * amtByWidth] = element;
                }
            }
        }

        return elements;
    }
}